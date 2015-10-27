using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSO2015Application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var amount = int.Parse(textBox1.Text);

            var combinations = FindNumberOfCombinations(amount, new List<int>(new int[] { 200, 100, 50, 20, 10, 5, 2, 1 }));

            MessageBox.Show("There are " + combinations + " combinations");
        }

        private int FindNumberOfCombinations(int amount, List<int> coins)
        {
            if (!coins.Any()) return 0;

            var combinations = 0;

            // The number you can make after taking an initial largest coin
            if (amount > coins.First())
                combinations = FindNumberOfCombinations(amount - coins.First(), coins);
            else if (amount == coins.First())
                combinations = 1;

            var subCoins = coins.Skip(1).ToList();

            combinations += FindNumberOfCombinations(amount, subCoins);

            return combinations;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var amount = int.Parse(textBox2.Text);
            var index = int.Parse(textBox3.Text);
            var coins = new List<int>(new int[] { 200, 100, 50, 20, 10, 5, 2, 1 });

            var combinations = FindExplicitCombinations(amount, coins);

            textBox10.Text = "";
            foreach (var combination in combinations)
                textBox10.Text += string.Join(",", combination.Select(x => x.ToString()).ToArray()) + Environment.NewLine;

            var wantedCombination = combinations.ElementAt(index - 1);

            MessageBox.Show("Wanted combination (in coins) " + index + " is " + 
               string.Join(",", wantedCombination.Select(x => x.ToString()).ToArray()));

            // Now turn into the desired output

            var output = new List<int>();
            foreach (var coin in coins)
            {
                var numberOfThatCoin = wantedCombination.Where(x => x == coin).Count();
                output.Add(numberOfThatCoin);
            }
            output.Reverse();

            MessageBox.Show("Output required: " + string.Join(" ", output.Select(x => x.ToString())));

        }

        // the lists are going to come out backwards...
        private List<List<int>> FindExplicitCombinations(int amount, List<int> coins)
        {
            if (!coins.Any()) return new List<List<int>>();

            var combinations = new List<List<int>>();

            // The number you can make after taking an initial largest coin
            if (amount > coins.First())
            {
                var Subcombinations = FindExplicitCombinations(amount - coins.First(), coins);
                foreach (var subcombination in Subcombinations)
                {
                    subcombination.Add(coins.First());
                    combinations.Add(subcombination);
                }

            }
            else if (amount == coins.First())
            {
                combinations = new List<List<int>>();
                combinations.Add(new List<int>() { amount });
            }

            var subCoins = coins.Skip(1).ToList();

            combinations.AddRange(FindExplicitCombinations(amount, subCoins));

            return combinations;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var bestProfit = 0;
            var bestBuyTime = 0;
            var bestSellTime = 0;

            var prices = textBox5.Text.Trim().Split(' ').Select(x => int.Parse(x));
            for (int i = 0; i < prices.Count() - 1; i++)
            {
                for (int j = i + 1; j < prices.Count(); j++)
                {
                    var profit = prices.ElementAt(j) - prices.ElementAt(i);
                    if (profit > bestProfit)
                    {
                        bestProfit = profit;
                        bestBuyTime = i;
                        bestSellTime = j;
                    }
                }

            }

            if (bestProfit == 0)
                MessageBox.Show("Bad luck, your stock fell continuously. No profit for you!");
            else
                MessageBox.Show(string.Format("Best profit {0}, made by buying at time {1} and selling at time {2}",
                    bestProfit, bestBuyTime + 1, bestSellTime + 1));

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var numbers = textBox6.Text.Trim().Split(' ').Select(x => int.Parse(x)).ToList();
            var desiredSolution = numbers.OrderBy(x => x).ToList();

            textBox10.Text = "";
            var swaps = FindMinimumNumberOfSwaps(numbers, desiredSolution);

            MessageBox.Show("I need " + swaps.Steps + " swaps");
            textBox10.Text = swaps.Desc;
        }

        private Sorting FindMinimumNumberOfSwaps(List<int> numbers, List<int> desiredSolution)
        {
            var minNumberOfSwapsAfterThisOne = Int32.MaxValue;
            string bestDesc = "";
            int bestI = 0, bestJ = 0;

            if (AreInOrder(numbers)) return new Sorting{ Steps = 0, Desc = ""};
            for (int i = 0; i < numbers.Count() - 1; i++)
                for (int j = i + 1; j < numbers.Count(); j++)
                {
                    // Ignore swapping same number
                    if (numbers[i] == numbers[j]) continue;
                    // Ignore if swap moves neither into right place
                    if (desiredSolution.ElementAt(i) != numbers.ElementAt(j)
                        && desiredSolution.ElementAt(j) != numbers.ElementAt(i))
                        continue;
                    // Ignore if swap moves one from right place to another right place
                    if (desiredSolution.ElementAt(i) == desiredSolution.ElementAt(j))
                        continue;

                    // Do the swap
                    int t = numbers[i]; numbers[i] = numbers[j]; numbers[j] = t;
                    var sortingAfterThisOne = FindMinimumNumberOfSwaps(numbers, desiredSolution);
                    var numberOfSwapsAfterThisOne = sortingAfterThisOne.Steps;
                    t = numbers[i]; numbers[i] = numbers[j]; numbers[j] = t;

                    if (numberOfSwapsAfterThisOne < minNumberOfSwapsAfterThisOne)
                    {
                        minNumberOfSwapsAfterThisOne = numberOfSwapsAfterThisOne;
                        bestDesc = sortingAfterThisOne.Desc;
                        bestI = i;
                        bestJ = j;
                    }
                }

            return new Sorting { 
                Steps = minNumberOfSwapsAfterThisOne + 1, 
                Desc = "swap " + (bestI+1) + "th and " + (bestJ+1) + "th" + Environment.NewLine +
                bestDesc };
        }

        private bool AreInOrder(List<int> numbers)
        {
            for (int i = 0; i < numbers.Count() - 1; i++)
                if (numbers.ElementAt(i) > numbers.ElementAt(i + 1))
                    return false;
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            var weights = textBox8.Text.Trim().Split(' ').Select(x => int.Parse(x)).ToList();
            var parms = textBox9.Text.Trim().Split(' ').Select(x => int.Parse(x));
            var numberOfItems = parms.First();
            var numberOfJourneys = parms.Last();

            textBox10.Text = "";

            var bestSolution = LowestMaxWeightPossible(weights, numberOfJourneys);

            MessageBox.Show("Best weight = " + bestSolution);
        }

        private int LowestMaxWeightPossible(List<int> weights, int journeysRemaining)
        {
            if (journeysRemaining > weights.Count())
                return weights.Max();
            if (journeysRemaining == 1)
                return weights.Sum();

            int best = Int32.MaxValue;

            for (int numOnFirst = 1; numOnFirst < weights.Count(); numOnFirst++)
            {
                int weightOnFirst = weights.Take(numOnFirst).Sum();
                int weightOnRest = LowestMaxWeightPossible(weights.Skip(numOnFirst).ToList(), journeysRemaining - 1);
                int max = Math.Max(weightOnFirst, weightOnRest);
                if (max <= best)
                    best = max;
            }

            textBox10.Text += string.Join(",", weights) + " in " + journeysRemaining + " => " + best + Environment.NewLine;

            return best;
        }
    }

    public class Sorting
    {
        public int Steps { get; set; }
        public string Desc { get; set; }
    }
}
