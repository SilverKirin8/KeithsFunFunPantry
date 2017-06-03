﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeithsFunFunPantry;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using KeithsFunFunPantry.CS;
using System.Collections.ObjectModel;

namespace AppUnitTests
{
	[TestClass]
	public class PantryTest
	{
		[TestMethod]
		public void AddNewIngredientTest_IngredientAddedToEndOfList()
		{
			Pantry.Ingredients = new ObservableCollection<Ingredient>();

			Pantry.AddNewIngredient("Milk", new Measurement(1f, Unit.Gallon));
			Assert.IsTrue(Pantry.Ingredients[Pantry.Ingredients.Count - 1].Name == "Milk");

			Pantry.AddNewIngredient("Sugar", new Measurement(1f, Unit.Pound));
			Assert.IsTrue(Pantry.Ingredients[0].Name == "Milk");
			Assert.IsTrue(Pantry.Ingredients[Pantry.Ingredients.Count - 1].Name == "Sugar");

			Pantry.AddNewIngredient("Baking Powder", new Measurement(24f, Unit.Tablespoon));
			Assert.IsTrue(Pantry.Ingredients[Pantry.Ingredients.Count - 1].Name == "Baking Powder");

			Pantry.AddNewIngredient("Salt", new Measurement(243.67f, Unit.Kilogram));
			Assert.IsTrue(Pantry.Ingredients[Pantry.Ingredients.Count - 1].Name == "Salt");
		}

		[TestMethod]
		public void AddNewIngredientTest_IngredientCount()
		{
			Pantry.Ingredients = new ObservableCollection<Ingredient>();

			Pantry.AddNewIngredient("Milk", new Measurement(1f, Unit.Gallon));
			Pantry.AddNewIngredient("Sugar", new Measurement(1f, Unit.Pound));
			Pantry.AddNewIngredient("Baking Powder", new Measurement(24f, Unit.Tablespoon));
			Pantry.AddNewIngredient("Salt", new Measurement(243.67f, Unit.Kilogram));

			Assert.AreEqual(4, Pantry.Ingredients.Count);
		}

		[TestMethod]
		public void RecipeNameSearchTest()
		{
			RecipeBook book = RecipeBook.Instance;

            List<Tag> tags = new List<Tag>()
            {
                Tag.Gluten,
                Tag.Indian,
                Tag.Vegan
            };

            List<Ingredient> ingredients = new List<Ingredient>()
			{
				new Ingredient("Milk", new Measurement(1f, Unit.Gallon)),
				new Ingredient("Bleu Cheese Dressing", new Measurement(16.7f, Unit.Ounce)),
				new Ingredient("Sugar", new Measurement(1f, Unit.Pound))
			};

			book.Recipes = new List<Recipe>()
			{
				new Recipe(tags, ingredients, "Chocolate Cake"),
				new Recipe(tags, ingredients, "Grilled Cheese"),
				new Recipe(tags, ingredients, "Pound Cake"),
				new Recipe(tags, ingredients, "Spaghetti"),
				new Recipe(tags, ingredients, "Shepard's Pie"),
				new Recipe(tags, ingredients, "Strawberry Cake"),
				new Recipe(tags, ingredients, "Banana Bread")
			};

			string query = "cake";
			List<Recipe> results = book.RecipeNameSearch(query);

			List<Recipe> expectedResults = new List<Recipe>()
			{
				new Recipe(tags, ingredients, "Chocolate Cake"),
				new Recipe(tags, ingredients, "Pound Cake"),
				new Recipe(tags, ingredients, "Strawberry Cake")
			};

			Assert.AreEqual(expectedResults.Count, results.Count);
			for (int i = 0; i < results.Count; i++)
			{
				Assert.AreEqual(expectedResults[i].ToString(), results[i].ToString());
			}
		}

		[TestMethod]
		public void IngredientNameSearchTest()
		{
			Pantry.Ingredients = new ObservableCollection<Ingredient>()
			{
				new Ingredient("Milk", new Measurement(1f, Unit.Gallon)),
				new Ingredient("Bleu Cheese Dressing", new Measurement(16.7f, Unit.Ounce)),
				new Ingredient("Sugar", new Measurement(1f, Unit.Pound)),
				new Ingredient("Baking Powder", new Measurement(24f, Unit.Tablespoon)),
				new Ingredient("Shredded Cheese", new Measurement(12f, Unit.Cup)),
				new Ingredient("Cheese", new Measurement(4.5f, Unit.Cup)),
				new Ingredient("Salt", new Measurement(243.67f, Unit.Kilogram))
			};

			string query = "cheese";
			ObservableCollection<Ingredient> results = Pantry.IngredientNameSearch(query);

			ObservableCollection<Ingredient> expectedResults = new ObservableCollection<Ingredient>()
			{
				new Ingredient("Bleu Cheese Dressing", new Measurement(16.7f, Unit.Ounce)),
				new Ingredient("Shredded Cheese", new Measurement(12f, Unit.Cup)),
				new Ingredient("Cheese", new Measurement(4.5f, Unit.Cup)),
			};

			Assert.AreEqual(expectedResults.Count, results.Count);
			for (int i = 0; i < results.Count; i++)
			{
				Assert.AreEqual(expectedResults[i].ToString(), results[i].ToString());
			}
		}



		[TestMethod]
		public void ConversionTest()
		{
			Measurement m = new Measurement(33f, Unit.FluidOunce);
			m.Convert(Unit.Cup);
			Assert.AreEqual(4.125f, m.Amount);

			m = new Measurement(33f, Unit.Pint);
			m.Convert(Unit.Cup);
			Assert.AreEqual(66f, m.Amount);

			m = new Measurement(23f, Unit.Quart);
			m.Convert(Unit.Cup);
			Assert.AreEqual(92f, m.Amount);

			m = new Measurement(13f, Unit.Gallon);
			m.Convert(Unit.Cup);
			Assert.AreEqual(208f, m.Amount);

			m = new Measurement(13f, Unit.Cup);
			m.Convert(Unit.FluidOunce);
			Assert.AreEqual(104f, m.Amount);

			m = new Measurement(23f, Unit.Cup);
			m.Convert(Unit.Pint);
			Assert.AreEqual(11.5f, m.Amount);

			m = new Measurement(23f, Unit.Cup);
			m.Convert(Unit.Quart);
			Assert.AreEqual(5.75f, m.Amount);

			m = new Measurement(33f, Unit.Cup);
			m.Convert(Unit.Gallon);
			Assert.AreEqual(2.062f, m.Amount);

			m = new Measurement(33f, Unit.FluidOunce);
			m.Convert(Unit.Pint);
			Assert.AreEqual(2.062f, m.Amount);

			m = new Measurement(33f, Unit.FluidOunce);
			m.Convert(Unit.Quart);
			Assert.AreEqual(1.031f, m.Amount);

			m = new Measurement(128f, Unit.FluidOunce);
			m.Convert(Unit.Gallon);
			Assert.AreEqual(1f, m.Amount);
		}
		[TestMethod]
		public void InvalidConversionTest()
		{
			Measurement m = new Measurement(1f, Unit.Cup);
			Measurement c = new Measurement(1f, Unit.Cup);

			//Same units
			m.Convert(Unit.Cup);
			Assert.AreEqual(m.UnitOfMeasurement, c.UnitOfMeasurement);

			m = new Measurement(1f, Unit.Gallon);
			c = new Measurement(1f, Unit.Gallon);
			m.Convert(Unit.Gallon);
			Assert.AreEqual(m.UnitOfMeasurement, c.UnitOfMeasurement);

			//Invalid conversions
			m.Convert(Unit.Count);
			Assert.AreEqual(m.UnitOfMeasurement, c.UnitOfMeasurement);

			m = new Measurement(1f, Unit.Count);
			c = new Measurement(1f, Unit.Count);
			m.Convert(Unit.FluidOunce);
			Assert.AreEqual(m.UnitOfMeasurement, c.UnitOfMeasurement);
		}

		[TestMethod]
		public void RecipeTagSearchTest()
		{
			List<Tag> tags = new List<Tag>()
			{
				Tag.Gluten,
				Tag.Indian,
				Tag.Vegan
			};

			List<Ingredient> ingredients = new List<Ingredient>()
			{
				new Ingredient("Milk", new Measurement(1f, Unit.Gallon)),
				new Ingredient("Bleu Cheese Dressing", new Measurement(16.7f, Unit.Ounce)),
				new Ingredient("Sugar", new Measurement(1f, Unit.Pound))
			};

			RecipeBook.Instance.Recipes = new List<Recipe>()
			{
				new Recipe(new List<Tag>(){Tag.American, Tag.Comfort}, ingredients, "Chocolate Cake"),
				new Recipe(new List<Tag>(){Tag.American}, ingredients, "Grilled Cheese"),
				new Recipe(new List<Tag>(){Tag.Gluten}, ingredients, "Pound Cake"),
				new Recipe(new List<Tag>(){Tag.Italian, Tag.Vegetarian}, ingredients, "Spaghetti"),
				new Recipe(new List<Tag>(){Tag.Vegetarian}, ingredients, "Shepard's Pie"),
				new Recipe(new List<Tag>(){Tag.Lactose}, ingredients, "Strawberry Cake"),
				new Recipe(new List<Tag>(){Tag.Comfort}, ingredients, "Banana Bread")
			};

			string query = "";
			List<Recipe> results = RecipeBook.Instance.RecipeNameSearch(query/*, new List<Tag>(){Tag.Lactose, Tag.Vegetarian, Tag.Gluten}*/);

			List<Recipe> expectedResults = new List<Recipe>()
			{
				new Recipe(new List<Tag>(){Tag.Gluten}, ingredients, "Pound Cake"),
				new Recipe(new List<Tag>(){Tag.Italian, Tag.Vegetarian}, ingredients, "Spaghetti"),
				new Recipe(new List<Tag>(){Tag.Vegetarian}, ingredients, "Shepard's Pie"),
				new Recipe(new List<Tag>(){Tag.Lactose}, ingredients, "Strawberry Cake")
			};

			Assert.AreEqual(expectedResults.Count, results.Count);
			Assert.AreEqual(expectedResults[1], results[1]);
			for (int i = 0; i < results.Count; i++)
			{
				Assert.AreEqual(expectedResults[i].ToString(), results[i].ToString());
			}
		}
	}
}
