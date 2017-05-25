﻿using KeithsFunFunPantry.CS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeithsFunFunPantry
{
    [Serializable()]
    public static class Pantry
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        private static ObservableCollection<Ingredient> ingredients = new ObservableCollection<Ingredient>();
        public static ObservableCollection<Ingredient> Ingredients
        {
            get { return ingredients; }
            set
            {
                ingredients = value;
            }
        }
        public static void AddNewIngredient(string name, Measurement m)
        {
            bool foundMatch = false;
            int count = Ingredients.Count();
            Ingredient i = new Ingredient(name, m);
            while (!foundMatch && count >= 0)
            {
                if (count == 0)
                {
                    Ingredients.Add(i);
                    Logging.WriteLog(LogLevel.Info, "Ingredient ' " + name + " ' added to list");
                    foundMatch = true;
                    break;
                }
                else if (Ingredients.ElementAt(count - 1).Name.ToLower() == i.Name.ToLower())
                {
                    Logging.WriteLog(LogLevel.Info, "Ingredient already exists in list");
                    Logging.WriteLog(LogLevel.Info, "Increasing the amount");
                    IncrementAmount(i);
                    foundMatch = true;
                }
                else
                {
                    count--;
                }
            }
        }
        private static void IncrementAmount(Ingredient i)
        {
            float addedAmount = i.IngredientMeasurement.Amount;
            for(int x = 0; x < Ingredients.Count(); x++)
            {
                if(i.Name == Ingredients[x].Name)
                {
                    Ingredients[x].IngredientMeasurement.Amount += addedAmount;
                    Logging.WriteLog(LogLevel.Info, "Ingredient " + Ingredients[x].Name + " amount incremented by " + i.IngredientMeasurement.Amount);
                }
            }
        }
        //Will export to file.
        public static void SaveIngredient()
        {
            try
            {
                using (Stream stream = File.Open("ingredients.xml", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, Ingredients);
                }
            }
            catch (IOException)
            {
                Logging.WriteLog(LogLevel.Error, "Pantry file has failed to open");
            }
        }
        //Will import all ingredients from file to list
        public static void ReadIngredientsFromFile()
        {
            try
            {
                using (Stream stream = File.Open("ingredients.xml", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    var ingredient = (ObservableCollection<Ingredient>)bin.Deserialize(stream);
                    var sortedIngredients = ingredient.OrderBy(a => a.Name);
                    Ingredients.Clear();
                    foreach(Ingredient ingredient1 in sortedIngredients)
                    {
                        Ingredients.Add(ingredient1);
                    }
                }
                Pantry.DisplayIngredients(Ingredients);
            }
            catch (IOException)
            {
                Logging.WriteLog(LogLevel.Error, "File has failed to open or doesn't exist");
            }
        }
        public static void RemoveIngredients(string name)
        {
            for(int x = 0; x < Ingredients.Count(); x++)
            {
                if (Ingredients[x].Name == name)
                {
                    Ingredients.Remove(Ingredients[x]);
                }
            }
            Logging.WriteLog(LogLevel.Info, "Ingredient " + name + " Removed");
        }

        public static void DisplayIngredients(ObservableCollection<Ingredient> i)
        {
            foreach (Ingredient ingredient in i)
            {
                Logging.WriteLog(LogLevel.Info, "Ingredient " + ingredient.Name + " added");
            }
        }

        #region Search Function

        //Controls ingredient-specific search function
        public static ObservableCollection<Ingredient> IngredientSearchController(string query/*, List<string> checkBoxValuesToFilter*/)
		{
			ObservableCollection<Ingredient> nameSearchResults = IngredientNameSearch(query);
            return nameSearchResults;
            //Leave until check box search is ready to be implemented
            //List<Ingredient> finalSearchResults = IngredientCheckBoxFilter(nameSearchResults, checkBoxValuesToFilter);
        }

		//Executes name search and returns the results
		public static ObservableCollection<Ingredient> IngredientNameSearch(string query)
		{
			ObservableCollection<Ingredient> queryResults = (ObservableCollection<Ingredient>)Ingredients.Where(ingredient => ingredient.Name.ToLower().Contains(query));
			return queryResults;
		}

		/*
		//Filters the given list based on the check box values and returns the filtered list
			//Narrow down nameSearchResults based on check boxes
		public static List<Ingredient> IngredientCheckBoxFilter(List<Ingredient> ingredientList, List<string> checkBoxValuesToFilter)
		{
			//for each String checkBoxValue, (linq) where ingredient.tags.contains(checkBoxValue)
			
			//Don't forget to check for duplicates
		}
		*/
		#endregion
	}
}
