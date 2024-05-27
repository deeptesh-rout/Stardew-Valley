using System;
using System.Collections.Generic;

// Define enum for different seasons
public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

// Define a class for crops
public class Crop
{
    public string Name { get; set; }
    public Season Season { get; set; }
    public int DaysToGrow { get; set; }
    public int SellingPrice { get; set; }
    public bool IsHarvested { get; set; } = false;

    public void Grow()
    {
        DaysToGrow--;
        if (DaysToGrow <= 0)
        {
            IsHarvested = true;
        }
    }
}

// Define a class for the player
public class Player
{
    public string Name { get; set; }
    public int Money { get; set; }
    public List<Crop> Inventory { get; set; } = new List<Crop>();
    public int Energy { get; set; } = 100; // Player's energy level
}

// Define a class for the farm
public class Farm
{
    public List<Crop> Crops { get; set; } = new List<Crop>();

    // Method to simulate the passage of time and crop growth
    public void PassDay()
    {
        foreach (Crop crop in Crops)
        {
            if (!crop.IsHarvested)
            {
                crop.Grow();
            }
        }
    }
}

// Define the main game class
public class StardewValleyGame
{
    public Player Player { get; set; }
    public Farm Farm { get; set; }

    public StardewValleyGame(string playerName)
    {
        // Initialize player and farm
        Player = new Player { Name = playerName };
        Farm = new Farm();
        InitializeCrops(); // Initialize farm with some crops
    }

    private void InitializeCrops()
    {
        // Add some initial crops to the farm
        Farm.Crops.Add(new Crop { Name = "Potato", Season = Season.Spring, DaysToGrow = 5, SellingPrice = 20 });
        Farm.Crops.Add(new Crop { Name = "Blueberry", Season = Season.Summer, DaysToGrow = 7, SellingPrice = 30 });
        Farm.Crops.Add(new Crop { Name = "Pumpkin", Season = Season.Fall, DaysToGrow = 1, SellingPrice = 50 });
        Farm.Crops.Add(new Crop { Name = "Cranberry", Season = Season.Fall, DaysToGrow = 6, SellingPrice = 40 });
        Farm.Crops.Add(new Crop { Name = "Winter Root", Season = Season.Winter, DaysToGrow = 8, SellingPrice = 25 });
        // Add more crops here as needed
    }

    // Method to start the game
    public void Start()
    {
        Console.WriteLine($"Welcome to Stardew Valley, {Player.Name}!");
        Console.WriteLine("You have inherited your grandfather's old farm. Let's start farming!");
        Console.WriteLine("Commands: plant <cropName>, harvest, pass, quit");
    }

    // Method to handle player input
    public void HandleInput(string input)
    {
        string[] inputs = input.Split(' ');
        string command = inputs[0].ToLower();

        switch (command)
        {
            case "plant":
                if (inputs.Length < 2)
                {
                    Console.WriteLine("Usage: plant <cropName>");
                    break;
                }
                PlantCrop(inputs[1]);
                break;
            case "harvest":
                HarvestCrops();
                break;
            case "pass":
                PassDay();
                break;
            case "quit":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid command.");
                break;
        }
    }

    private void PlantCrop(string cropName)
    {
        // Check if the player has the crop in inventory
        Crop cropToPlant = Player.Inventory.Find(c => c.Name.ToLower() == cropName.ToLower());
        if (cropToPlant == null)
        {
            Console.WriteLine($"You don't have {cropName} in your inventory.");
            return;
        }

        // Remove the crop from inventory and add it to the farm
        Player.Inventory.Remove(cropToPlant);
        Farm.Crops.Add(cropToPlant);
        Console.WriteLine($"You planted {cropName} on your farm.");
    }

    private void HarvestCrops()
    {
        int totalMoney = 0;
        List<Crop> harvestedCrops = new List<Crop>(); // Create a new list to store harvested crops
        foreach (Crop crop in Farm.Crops)
        {
            if (crop.IsHarvested)
            {
                totalMoney += crop.SellingPrice;
                harvestedCrops.Add(crop); // Add the harvested crop to the new list
            }
        }
        // Remove harvested crops from the farm
        foreach (Crop crop in harvestedCrops)
        {
            Farm.Crops.Remove(crop);
        }
        Player.Money += totalMoney;
        Console.WriteLine($"You harvested crops and earned {totalMoney} gold.");
    }

    private void PassDay()
    {
        Player.Energy -= 10; // Deduct some energy for passing the day
        if (Player.Energy <= 0)
        {
            Console.WriteLine("You are out of energy and cannot work anymore today.");
            return;
        }

        Farm.PassDay(); // Simulate crop growth
        Console.WriteLine("A day has passed on your farm.");
    }

    // Method to update game state
    public void Update()
    {
        // Additional game state update logic can be added here
    }

    // Method to display game state
    public void Render()
    {
        Console.WriteLine($"Player: {Player.Name} | Money: {Player.Money} | Energy: {Player.Energy}");
        Console.WriteLine("Crops on Farm:");
        foreach (Crop crop in Farm.Crops)
        {
            Console.WriteLine($"{crop.Name} - {(crop.IsHarvested ? "Ready to harvest" : $"{crop.DaysToGrow} days to grow")}");
        }
    }
}

// Main class to run the game
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter your name:");
        string playerName = Console.ReadLine();

        StardewValleyGame game = new StardewValleyGame(playerName);
        game.Start();

        // Main game loop
        while (true)
        {
            game.Render();
            Console.WriteLine("Enter command:");
            game.HandleInput(Console.ReadLine());
            game.Update();
        }
    }
}
