using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab3_Master
{
    public class Neuron
    {
        private int _id = 0;
        private List<double> _weights;

        public int id
        {
            get
            {
                return _id;
            }

            private set
            {
                _id = value;
            }
        }

        public List<double> weights
        {
            get
            {
                return _weights;
            }

            private set
            {
                _weights = value;
            }
        }

        public Neuron(int id, int count_of_inputs)
        {
            weights = new List<double>();
            this.id = id;
            Random rand = new Random();
            for (int i = 0; i < count_of_inputs; i++)
            {
                weights.Add(rand.NextDouble());
            }

        }
    }
}
