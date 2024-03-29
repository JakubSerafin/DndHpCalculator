﻿using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;
using DND_HP_API.Domain.Repositories;
using DND_HP_API.Infrastructure.DbModels;
using DnDHpCalculator.Database;
using Newtonsoft.Json;

namespace DND_HP_API.Infrastructure;

public class CharacterSheetSqlLittleRepository : ICharacterSheetRepository
{
    public ICollection<CharacterSheet> GetAll()
    {
        using (var database = SqlLiteDatabase.GetConnection())
        {
            using (var command = database.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CharacterSheets";

                using (var reader = command.ExecuteReader())
                {
                    var characterSheets = new List<CharacterSheet>();
                    while (reader.Read())
                    {
                        var data = reader.GetString(1);
                        var characterSheetDb = JsonConvert.DeserializeObject<CharacterSheetDbModel>(data)
                                               ??throw new Exception("Failed to deserialize character sheet");
                        var characterSheet = characterSheetDb.BuildModel(new Id(reader.GetInt32(0)));
                        characterSheets.Add(characterSheet);
                    }

                    return characterSheets;
                }
            }
        }
    }

    public CharacterSheet? Get(int id)
    {
        using (var database = SqlLiteDatabase.GetConnection())
        {
            using (var command = database.CreateCommand())
            {
                command.CommandText = "SELECT Data FROM CharacterSheets WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var data = reader.GetString(0);
                        var characterSheetDb = JsonConvert.DeserializeObject<CharacterSheetDbModel>(data)
                                               ??throw new Exception("Failed to deserialize character sheet");
                        var characterSheet = characterSheetDb.BuildModel(new Id(id)); 
                        return characterSheet;
                    }
                }
            }
        }

        return null;
    }

    public Id Add(CharacterSheet item)
    {
        using (var database = SqlLiteDatabase.GetConnection())
        {
            long? existingRecordId = !item.Id.IsTemporary ? item.Id.Value : null;
            long resultId;
            using (var command = database.CreateCommand())
            {
                var data = JsonConvert.SerializeObject(CharacterSheetDbModel.BuildFromEntity(item));
                if (existingRecordId.HasValue)
                {
                    command.CommandText = @"UPDATE CharacterSheets SET Data = $data WHERE Id = $id";
                    command.Parameters.AddWithValue("$id", existingRecordId.Value);
                }
                else
                {
                    command.CommandText = @"INSERT INTO CharacterSheets (Data) VALUES ($data);
                    select last_insert_rowid()";
                }

                command.Parameters.AddWithValue("$data", data);
                var rowResult = (long?)command.ExecuteScalar();
                if (existingRecordId.HasValue)
                    resultId = existingRecordId.Value;
                else
                    resultId = (int)(rowResult??throw new Exception("Failed to get last inserted id"));
            }

            return new Id(resultId);
        }
    }

    public bool Delete(int id)
    {
        using (var database = SqlLiteDatabase.GetConnection())
        {
            using (var command = database.CreateCommand())
            {
                command.CommandText = "DELETE FROM CharacterSheets WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}