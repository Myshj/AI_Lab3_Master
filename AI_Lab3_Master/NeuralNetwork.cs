using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab3_Master
{
    public class NeuralNetwork
    {
        private int _count_of_inputs = 0;
        public List<Neuron> neurons;

        public int count_of_inputs
        {
            get
            {
                return _count_of_inputs;
            }

            set
            {
                _count_of_inputs = value;
            }
        }

        public NeuralNetwork(int count_of_inputs, int count_of_neurons)
        {
            neurons = new List<Neuron>();
            this.count_of_inputs = count_of_inputs;
            for (int i = 0; i < count_of_neurons; i++)
            {
                Neuron neuron = new Neuron(i, count_of_inputs);
                neurons.Add(neuron);
            }
        }

    }
}
