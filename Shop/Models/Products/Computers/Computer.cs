using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineShop.Common.Constants;
using OnlineShop.Models.Products.Components;
using OnlineShop.Models.Products.Peripherals;

namespace OnlineShop.Models.Products.Computers
{
    public abstract class Computer : Product, IComputer
    {
        private readonly List<IComponent> components;
        private readonly List<IPeripheral> peripherals;

        public Computer(int id, string manufacturer, string model, decimal price, double overallPerformance) 
            : base(id, manufacturer, model, price, overallPerformance)
        {
            components = new List<IComponent>();
            peripherals = new List<IPeripheral>();
        }

        public IReadOnlyCollection<IComponent> Components => components;

        public IReadOnlyCollection<IPeripheral> Peripherals => peripherals;

        public override double OverallPerformance => base.OverallPerformance + (components.Sum(o => o.OverallPerformance) / components.Count);

        public override decimal Price => base.Price + components.Sum(p => p.Price) + peripherals.Sum(c => c.Price);

        public void AddComponent(IComponent component)
        {
            IComponent newcomponent = components.FirstOrDefault(x => x.GetType().Name == component.GetType().Name);

            if (newcomponent != null)
            {
                throw new ArgumentException($"Component {component.GetType().Name} already exists in {this.GetType().Name} with Id {Id}.");
            }

            components.Add(newcomponent);
        }

        public void AddPeripheral(IPeripheral peripheral)
        {
            IPeripheral newPeripheral = peripherals.FirstOrDefault(x => x.GetType().Name == peripheral.GetType().Name);

            if (newPeripheral != null)
            {
                throw new ArgumentException($"Peripheral {peripheral.GetType().Name} already exists in {this.GetType().Name} with Id {Id}.");
            }

            peripherals.Add(newPeripheral);
        }

        public IComponent RemoveComponent(string componentType)
        {
            if (components.Count == 0 || components.FirstOrDefault(x => x.GetType().Name == componentType) == null)
            {
                throw new ArgumentException($"Component {componentType} does not exist in {this.GetType().Name} with Id {Id}.");
            }

            IComponent component = components.FirstOrDefault(x => x.GetType().Name == componentType);

            components.Remove(component);
            return component;
        }

        public IPeripheral RemovePeripheral(string peripheralType)
        {
            IPeripheral peripheral = peripherals.FirstOrDefault(x => x.GetType().Name == peripheralType);

            if (peripherals.Count == 0 || peripheral == null)
            {
                throw new ArgumentException($"Peripheral {peripheralType} does not exist in {this.GetType().Name} with Id {Id}.");
            }

            peripherals.Remove(peripheral);
            return peripheral;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Overall Performance: {OverallPerformance}. Price: {Price} - {GetType().Name}: {Manufacturer} {Model} (Id: {Id})");
            sb.AppendLine($" Components({components.Count}):");

            foreach (IComponent component in components)
            {
                sb.AppendLine($"  {component.ToString()}");
            }

            sb.AppendLine($" Peripherals ({peripherals.Count}); Average Overall Performance ({peripherals.Average(a => a.OverallPerformance)}):");

            foreach (IPeripheral peripheral in peripherals)
            {
                sb.AppendLine($"  {peripheral.ToString()}");
            }

            return sb.ToString().Trim();
        }
    }
}
