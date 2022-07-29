using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Characters;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Items;

namespace WarCroft.Core
{
    public class WarController
	{
        private List<Character> party;
        private List<Item> pool;

		public WarController()
		{
            party = new List<Character>();
            pool = new List<Item>();
		}

		public string JoinParty(string[] args)
		{
            string characterType = args[0];
            string name = args[1];

            Character character = null;

            if (characterType == "Warrior")
            {
                character = new Warrior(name);
            }
            else if (characterType == "Priest")
            {
                character = new Priest(name);
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidCharacterType, name);
            }

            party.Add(character);
            return $"{name} joined the party!";
        }

		public string AddItemToPool(string[] args)
		{
            string itemName = args[0];

            Item item = null;

            if (itemName == "FirePotion")
            {
                item = new FirePotion();
            }
            else if (itemName == "HealthPotion")
            {
                item = new HealthPotion();
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidItem, itemName);     
            }

            pool.Add(item);
            return $"{itemName} added to pool.";
        }

		public string PickUpItem(string[] args)
		{
            string characterName = args[0];

            Character character = party.FirstOrDefault(x => x.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException(ExceptionMessages.CharacterNotInParty, characterName);
            }

            if (pool.Count == 0)
            {
                throw new InvalidOperationException(ExceptionMessages.ItemPoolEmpty);
            }

            Item item = pool[pool.Count - 1];
            character.Bag.AddItem(item); // dali se maxa itema
            pool.RemoveAt(pool.Count - 1);
            return $"{characterName} picked up {item.GetType().Name}!"; // eventualno dali ne e samo GetType()
        }

		public string UseItem(string[] args)
		{
			string characterName = args[0];
            string itemName = args[1];

            Character character = party.FirstOrDefault(x => x.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException(ExceptionMessages.CharacterNotInParty, characterName);
            }

            Item item = character.Bag.GetItem(itemName); // eventualno

            if (item != null)
            {
                character.UseItem(item); // dali se maxa
                return $"{character.Name} used {itemName}.";
            }

            return null;
        }

		public string GetStats()
		{
            StringBuilder sb = new StringBuilder();
            string aliveOrNot = string.Empty;

            foreach (var character in party.OrderByDescending(x => x.IsAlive == true).ThenByDescending(h => h.Health))
            {
                if (!character.IsAlive)
                {
                    aliveOrNot = "Dead";
                }
                else
                {
                    aliveOrNot = "Alive";
                }

                string line = string.Format("{0} - HP: {1}/{2}, AP: {3}/{4}, Status: {5}"
                    ,character.Name
                    ,character.Health
                    ,character.BaseHealth
                    ,character.Armor
                    ,character.BaseArmor
                    ,aliveOrNot);

                sb.AppendLine(line);
            }

            return sb.ToString().Trim();
		}

		public string Attack(string[] args)
		{
            string attackerName = args[0];
            string receiverName = args[1];

            string output = string.Empty;

            Character attacker = party.FirstOrDefault(n => n.Name == attackerName);
            Character receiver = party.FirstOrDefault(k => k.Name == receiverName);

            if (attacker == null)
            {
                throw new ArgumentException(ExceptionMessages.CharacterNotInParty, attackerName);
            } // eventualno s else if

            if (receiver == null)
            {
                throw new ArgumentException(ExceptionMessages.CharacterNotInParty, receiverName);
            }

            if (attacker is Warrior warrior)
            {
                if (!warrior.IsAlive)
                {
                    throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
                }

                warrior.Attack(receiver);

               output = string.Format("{0} attacks {1} for {2} hit points! {3} has {4}/{5} HP and {6}/{7} AP left!"
                    ,attackerName,
                    receiverName,
                    attacker.AbilityPoints,
                    receiverName,
                    receiver.Health,
                    receiver.BaseHealth,
                    receiver.Armor,
                    receiver.BaseArmor
                    );

                if (!receiver.IsAlive)
                {
                    output += $"\n{receiver.Name} is dead!";
                }

                return output;
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.AttackFail, attackerName);
            }
        }

		public string Heal(string[] args)
		{
            string healerName = args[0];
            string healingReceiverName = args[1];

            string output = string.Empty;

            Character healer = party.FirstOrDefault(n => n.Name == healerName);
            Character receiver = party.FirstOrDefault(k => k.Name == healingReceiverName);

            if (healer == null)
            {
                throw new ArgumentException(ExceptionMessages.CharacterNotInParty, healerName);
            } // eventualno s else if

            if (receiver == null)
            {
                throw new ArgumentException(ExceptionMessages.CharacterNotInParty, healingReceiverName);
            }

            if (healer is Priest priest)
            {

                if (!priest.IsAlive)
                {
                    throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
                }

                priest.Heal(receiver);

                output = string.Format("{0} heals {1} for {2}! {3} has {4} health now!"
                    ,healer.Name
                    ,receiver.Name
                    ,healer.AbilityPoints
                    ,receiver.Name
                    ,receiver.Health);

                return output;
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.HealerCannotHeal, healerName);
            }
        }
	}
}
