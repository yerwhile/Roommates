﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT rm.FirstName, rm.RentPortion, r.Name, r.Id
                                        FROM Roommate rm
                                            JOIN Room r ON r.Id = rm.RoomId
                                        WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;
                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                }
                            };
                    
                        }
                        return roommate;
                    }
                }
            }
        }

        public List<Roommate> GetAllExcept(int roommateId)
        {
            List<Roommate> allRoommates = new List<Roommate>();
            allRoommates = GetAll();
            List<Roommate> allExcept = new List<Roommate>();
            foreach(var rm in allRoommates)
            {
                if(rm.Id != roommateId)
                {
                    allExcept.Add(rm);
                }
            }
            return allExcept;
        }

        public List<Roommate> GetByChore(int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT rm.id, rm.FirstName, rm.LastName
                                            FROM Roommate rm
                                               JOIN RoommateChore rmc
                                                    ON rm.Id = rmc.RoommateId
                                         WHERE rmc.ChoreId = @choreId";
                    cmd.Parameters.AddWithValue("@choreId", choreId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();

                        while(reader.Read())
                        {
                            Roommate roommate = new Roommate()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };
                            roommates.Add(roommate);
                            
                        }
                        return roommates;
                    }
                }
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName FROM Roommate";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();

                        while (reader.Read())
                        {

                            Roommate roommate = new Roommate
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };

                            roommates.Add(roommate);
                        }

                        return roommates;
                    }
                }
            }
        }
    }
}
