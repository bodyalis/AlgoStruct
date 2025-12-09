namespace HuffmanAlgorithm.Univer.Lab_1;

// Дано множество предметов. Каждый предмет характеризуется двумя
// параметрами:
//     • Вес 𝑝!
//     • Ценность 𝑣!
//     Разрешается брать предмет не полностью, а только некоторую его часть.
//     При этом ценность и вес части предмета пропорциональны величине взятой
// доли.
//     Имеется рюкзак, максимальная грузоподъёмность которого равна p+,-.
// Необходимо выбрать набор предметов (возможно, с использованием долей
//     некоторых из них) так, чтобы суммарный вес набора не превышал допустимый
// вес рюкзака, а суммарная ценность выбранных предметов была максимально
// возможной. Каждый предмет можно брать не более одного раза
// № предмета Вес 𝑝! Ценность 𝑣! Грузоподъёмность p+,-
// 1            10      40          135
// 2            20      90
// 3            35      120
// 4            40      100
// 5            50      80

public class Task_1
{
    public class Item
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public double Value { get; set; }
        public double Ratio => Value / Weight;
    }

    private List<Item> items = new List<Item>
    {
        new() { Id = 1, Weight = 10, Value = 40 },  // Ratio: 4.0
        new() { Id = 2, Weight = 20, Value = 90 },  // Ratio: 4.5
        new() { Id = 3, Weight = 35, Value = 120 }, // Ratio: 3.43
        new() { Id = 4, Weight = 40, Value = 100 }, // Ratio: 2.5
        new() { Id = 5, Weight = 50, Value = 80 }   // Ratio: 1.6
    };
    
    private const int MaxCapacity = 135;
    public Task_1()
    {
        
    }

    public List<(Item item, double fraction)> Solve()
    {
        var sortedItems = items.OrderByDescending(i => i.Ratio);

        List<(Item item, double fraction)> addedItems = new List<(Item item, double fraction)>();
        double remainingCapacity = MaxCapacity;
        foreach (var item in sortedItems)
        {
            if (remainingCapacity == 0)
            {
                break;
            }
            
            if (remainingCapacity >= item.Weight)
            {
                addedItems.Add((item, 1.0d));
                remainingCapacity -= item.Weight;
            }
            else
            {
                double fraction = remainingCapacity / item.Weight;
                addedItems.Add((item, fraction));
                remainingCapacity = 0;
            }
        }

        return addedItems;
    }
}