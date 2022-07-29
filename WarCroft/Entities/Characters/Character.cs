using System;

using WarCroft.Constants;
using WarCroft.Entities.Inventory;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Characters.Contracts
{
    public abstract class Character
    {
        private string name;
        private double health;
        private double armor;

        public Character(string name, double health, double armor, double abilityPoints, Bag bag)
        {
            Name = name;
            BaseHealth = health;
            Health = BaseHealth; // eventualno
            BaseArmor = armor;
            Armor = BaseArmor; // eventualno
            AbilityPoints = abilityPoints;
            Bag = bag;
            IsAlive = true;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.CharacterNameInvalid);
                }

                name = value;
            }
        }

        public double BaseHealth { get; private set; }

        public double Health
        {
            get => health;
            set
            {
                if (value > BaseHealth) // eventualno
                {
                    health = BaseHealth;
                }
                else if (value <= 0)
                {
                    health = 0;
                    IsAlive = false;
                }
                else
                {
                    health = value;
                }
            }
        }

        public double BaseArmor { get; private set; }

        public double Armor
        {
            get => armor;
            set // eventualno
            {
                if (value < 0)
                {
                    this.armor = 0;
                }
                else if (value > this.BaseArmor)
                {
                    this.armor = this.BaseArmor;
                }
                else
                {
                    this.armor = value;
                }
            }
        }

        public double AbilityPoints { get; private set; }

        public Bag Bag { get; private set; }

        public bool IsAlive { get; set; }
        

        protected void EnsureAlive()
        {
            if (!this.IsAlive)
            {
                throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
            }
        }

        public void TakeDamage(double hitPoints)
        {
            EnsureAlive();

            if (Armor >= hitPoints)
            {
                Armor -= hitPoints;
            }
            else
            {
                hitPoints -= Armor;
                Armor = 0;
                Health -= hitPoints;
            }

            if (health <= 0)
            {
                IsAlive = false;
            }
        }

        public void UseItem(Item item)
        {
            EnsureAlive();
            item.AffectCharacter(this);
        }
    }
}