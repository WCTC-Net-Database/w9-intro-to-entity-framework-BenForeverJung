using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;

namespace W9_assignment_template.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();
        if (rooms.Any())
        {
            Console.WriteLine("\nRooms:");
            foreach (var room in rooms)
            {
                Console.WriteLine($"Room: #{room.Id}. {room.Name} - {room.Description}");
                foreach (var character in room.Characters)
                {
                    Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
                }
            }
        }
        else
        {
            Console.WriteLine("No rooms available.");
        }
        Console.WriteLine("---                                                  ---");
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters)
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
        Console.WriteLine("---                                                  ---");
    }
    // Create method to add new room from user input
    public void AddRoom()
    {
        //var roomList = _context.Rooms.ToList();
        Console.WriteLine();
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();

        Console.Write("Enter room description: ");
        var description = Console.ReadLine();
        Console.WriteLine();
        if (name != null)
        {
            if (_context.Rooms.Any(rm => rm.Name.ToLower() != name.ToLower()))
            {
                var room = new Room
                {
                    Name = name,
                    Description = description
                };

                _context.Rooms.Add(room);
                _context.SaveChanges();

                Console.WriteLine($"Room '{name}' added to the game.");
            }
            else
            {
                Console.WriteLine($"The room '{name}' already exists.  Cannot duplicate rooms with the same name.");

            }
        }
        else
        {
            Console.WriteLine($"Cannot add a room without a name.");
        }
        Console.WriteLine("---                                                  ---");
    }

    // Create method to add a new character from user input
    public void AddCharacter()
    {
        Console.WriteLine();
        Console.Write("Enter character name: ");
        var name = Console.ReadLine();

        Console.Write("Enter character level: ");
        var level = int.Parse(Console.ReadLine());

        Console.WriteLine();
        Console.WriteLine("Available rooms:");
        DisplayRooms();
        Console.Write("Enter room ID for the character: ");
        var roomId = int.Parse(Console.ReadLine());

        // Add character to the room
        // Find the room by ID
        // Check If the room doesn't exist, return
        // Otherwise, create a new character and add it to the room
        // Save the changes to the database
        Console.WriteLine();
        if (_context.Rooms.Any(rm => rm.Id == roomId))
        {
            if (name != null)
            {
                if (_context.Characters.Any(ch => ch.Name.ToLower() == name.ToLower()))
                {
                    Console.WriteLine(
                        $"The character '{name}' already exists.  Cannot duplicate characters with the same name.");
                }
                else
                {
                    var character = new Character()
                    {
                        Name = name,
                        Level = level,
                        RoomId = roomId
                    };

                    _context.Characters.Add(character);
                    _context.SaveChanges();

                    Console.WriteLine($"Character '{name}' added to the game.");
                }
            }
            else
            {
                Console.WriteLine($"Cannot add a room without a name.");
            }
        }
        else
        {
            Console.WriteLine($"Room ID {roomId} does not exist.  Cannot add a character without a room.");
        }
        Console.WriteLine("---                                                  ---");
    }

    // Create method to Find a character by name

    public void FindCharacter()
    {
        Console.WriteLine();
        Console.Write("Enter character name to search: ");
        var findCharacter = Console.ReadLine();

        // Use LINQ to query the database for the character
        var foundCharacter = _context.Characters.FirstOrDefault(foundCh => foundCh.Name.ToLower() == findCharacter.ToLower());
        
        // If the character exists, display the character's information
        if (foundCharacter != null)
        {
            Console.WriteLine($"Character ID: {foundCharacter.Id}, Name: {foundCharacter.Name}, Level: {foundCharacter.Level}, Room ID: {foundCharacter.RoomId}");
        }
        // Otherwise, display a message indicating the character was not found
        else
        {
            Console.WriteLine($"Could not find {findCharacter} in the character system");
        }
        Console.WriteLine("---                                                  ---");
    }


}