using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> deleteRooms = roomRepo.GetAll();
                        foreach(Room r in deleteRooms)
                        {
                            Console.WriteLine($"{r.Id}. {r.Name}");
                        }
                        Console.Write("Choose a room by Id to delete:");
                        int deleteChoice = int.Parse(Console.ReadLine());
                        Console.WriteLine("Are you sure you want to delete this room? y/n: ");
                        string deleteConfirm = Console.ReadLine().ToLower();
                        if( deleteConfirm == "y" )
                        {
                            roomRepo.Delete(deleteChoice);
                        }
                        else
                        {
                            break;
                        }
                        Console.WriteLine("Room has been successfully deleted!");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach(Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}!");
                        }
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);
                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                        break;
                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(c => c.Id == selectedChoreId);

                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine();

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Reassign chore"):
                        List<Chore> assignedChores = new List<Chore>();
                        assignedChores = choreRepo.GetAssignedChores();
                        Console.WriteLine("Choose an assigned chore by Id to re-assign:");
                        foreach(var assignedChore in assignedChores)
                        {
                            Console.WriteLine($"{assignedChore.Id}. {assignedChore.Name}");
                        }
                        int reassignChoreChoice = int.Parse(Console.ReadLine());
                        List<Roommate> assignedRoommates = new List<Roommate>();
                        assignedRoommates = roommateRepo.GetByChore(reassignChoreChoice);
                        int unassignChoice = 0;
                        if(assignedRoommates.Count == 1)
                        {
                            Console.WriteLine($"This chore is currently assigned to {assignedRoommates[0].FirstName} {assignedRoommates[0].LastName}. Who would you like to assign it to?");
                            unassignChoice = assignedRoommates[0].Id;
                        }
                        else
                        {
                            Console.WriteLine($"This chore is currently assigned to {assignedRoommates.Count} Roommates. Choose a Roommate by Id whom you'd like to replace:");
                            foreach(var rm in assignedRoommates)
                            {
                                Console.WriteLine($"{rm.Id}. {rm.FirstName} {rm.LastName}");
                            }
                            unassignChoice = int.Parse(Console.ReadLine());
                            Console.WriteLine("Now choose who would you like to assign this chore to");
                        }
                        int assignChoice = 0;
                        List<Roommate> availableRoommates = new List<Roommate>();
                        availableRoommates = roommateRepo.GetAllExcept(unassignChoice);
                        foreach(var rm in availableRoommates)
                        {
                            Console.WriteLine($"{rm.Id}. {rm.FirstName} {rm.LastName}");
                        }
                        assignChoice = int.Parse(Console.ReadLine());
                        choreRepo.Reassign(unassignChoice, assignChoice, reassignChoreChoice);
                        Console.WriteLine("Chore has been successfully reassigned!");
                        Console.Write("Press any Key to continue:");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"):
                        List<Chore> deleteChores = choreRepo.GetAll();
                        foreach (Chore c in deleteChores)
                        {
                            Console.WriteLine($"{c.Id}. {c.Name}");
                        }
                        Console.Write("Choose a chore by Id to delete:");
                        int deleteChoreChoice = int.Parse(Console.ReadLine());
                        Console.WriteLine("Are you sure you want to delete this room? y/n: ");
                        string deleteChoreConfirm = Console.ReadLine().ToLower();
                        if (deleteChoreConfirm == "y")
                        {
                            choreRepo.Delete(deleteChoreChoice);
                        }
                        else
                        {
                            break;
                        }
                        Console.WriteLine("Chore has been successfully deleted!");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);
                        Console.WriteLine($"{roommate.FirstName} pays ${roommate.RentPortion} rent and stays in {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("See unassigned chores"):
                        List < Chore > unChores = choreRepo.GetUnassignedChores();
                        foreach(Chore unChore in unChores)
                        {
                            Console.WriteLine($"{unChore.Name} is unassigned");
                        }
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                        Console.WriteLine("List of Unassigned Chores:");
                        List<Chore> unChoreOptions = choreRepo.GetUnassignedChores();
                        foreach(Chore unChoreOption in unChoreOptions)
                        {
                            Console.WriteLine($"{unChoreOption.Id}. {unChoreOption.Name}");
                        }
                        Console.WriteLine("Choose the Chore Id to assign: ");
                        int choreChoiceId = int.Parse(Console.ReadLine());
                        Console.WriteLine("List of Roommates: ");
                        List<Roommate> roommateOptions = roommateRepo.GetAll();
                        foreach(Roommate roommateOption in roommateOptions)
                        {
                            Console.WriteLine($"{roommateOption.Id}. {roommateOption.FirstName} {roommateOption.LastName}");
                        }
                        Console.WriteLine("Choose the Roommate Id to assign chore: ");
                        int roommateChoiceId = int.Parse(Console.ReadLine());
                        choreRepo.AssignChore(roommateChoiceId, choreChoiceId);
                        Console.WriteLine("This lazy roommate is now occupied.");
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                        break;
                    case ("Get chore count"):
                        Console.WriteLine("A list of roommates and how many chores they better do or else:");
                        List<ChoreCount> choreCounts = choreRepo.GetChoreCounts();
                        foreach(ChoreCount choreCount in choreCounts)
                        {
                            Console.WriteLine($"{choreCount.Roommate.FirstName} has {choreCount.NumChores} chores they're either workin' on or shirkin' on.");
                        }
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

            static string GetMenuSelection()
            {
                Console.Clear();

                List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Update a chore",
                "Reassign chore",
                "Delete a chore",
                "Search for roommate",
                "See unassigned chores",
                "Assign chore to roommate",
                "Get chore count",
                "Exit"
            };

                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i]}");
                }

                while (true)
                {
                    try
                    {
                        Console.WriteLine();
                        Console.Write("Select an option > ");

                        string input = Console.ReadLine();
                        int index = int.Parse(input) - 1;
                        return options[index];
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }
            }
        }
    }
}
