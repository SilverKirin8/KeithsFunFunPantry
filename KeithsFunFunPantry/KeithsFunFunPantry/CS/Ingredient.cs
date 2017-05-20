﻿using KeithsFunFunPantry.CS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace KeithsFunFunPantry
{
    [Serializable()]
    public class Ingredient
    {
        public event PropertyChangedEventHandler PropertyChanged;
        Regex nameValidation = new Regex(@"^[a-zA-Z/s]*$");
        private string name = "";
        private Measurement ingredientMeasurement;
        private List<string> tags = new List<string>();

        //Name of the ingredient
        public string Name
        {
            get { return name; }
            set
            {
                if (nameValidation.IsMatch(name))
                {
                name = value;
                FieldChanged();
                }
                else
                {
                    MessageBox.Show("Please enter a valid name for the ingredient.");
                    Logging.WriteLog(LogLevel.Warning, "Invalid ingredient name entered" + name);
                }
            }
        }

        //Measurement of the ingredient
        public Measurement IngredientMeasurement
        {
            get { return ingredientMeasurement; }
            set
            {
                ingredientMeasurement = value;
                FieldChanged();
            }
        }

        //the List of Tags corresponding to the ingredient
        public List<string> Tags
        {
            get { return tags; }
            set
            {
                tags = value;
                FieldChanged();
            }
        }

        public Ingredient(string name, Measurement m)
        {
            Name = name;
            IngredientMeasurement = m;
        }

        public override string ToString()
        {
            return Name + " " + IngredientMeasurement;
        }

        protected void FieldChanged([CallerMemberName] string field = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }
    }
}