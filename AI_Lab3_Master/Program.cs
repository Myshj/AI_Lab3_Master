using System;
using System.Collections.Generic;
using System.Drawing;

namespace AI_Lab3_Master
{
    static class Program
    {
        private static int count_of_learning_images = 3;
        private static int count_of_test_images = 6;
        private static int count_of_inputs = 45 * 45;
        private static int count_of_epochs = 50;

        private static void run()
        {
            int count_of_outputs = count_of_learning_images;
            List<List<double>> base_images = new List<List<double>>(count_of_outputs);
            for (int i = 0; i < count_of_outputs; i++)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "base_" + i.ToString() + ".png";
                Bitmap image = new Bitmap(System.Drawing.Image.FromFile(path, true));
                base_images.Add(convert_bitmap_to_list(image));
            }
            NeuralNetwork net = new NeuralNetwork(count_of_inputs, count_of_outputs);
            teach_network(ref net, base_images);
            for (int i = 0; i < count_of_test_images; i++)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "real_" + i.ToString() + ".png";
                Bitmap image = new Bitmap(System.Drawing.Image.FromFile(path, true));
                Console.WriteLine("Real image number " + i + " recognized as: " + test(net, convert_bitmap_to_list(image)).ToString());
            }
        }

        private static List<double> convert_bitmap_to_list(Bitmap image)
        {
            int size = image.Size.Height * image.Size.Width;
            List<double> ret = new List<double>(size);
            int i = 0;
            for (int column = 0; column < image.Size.Width; column++)
            {
                for (int row = 0; row < image.Size.Height; row++)
                {
                    Color pixel = image.GetPixel(column, row);
                    var luminance = pixel.R * 0.2126 + pixel.G * 0.7152 + pixel.B * 0.0722;
                    ret.Add(1.0f - luminance / 255.0f);
                }
            }
            return ret;
        }

        private static int test(NeuralNetwork neural_network, List<double> inputs)
        {
            double min_distance = Double.MaxValue;
            int bmu_index = 0;
            for (int i = 0; i < neural_network.neurons.Count; i++)
            {
                double distance = calculate_distance(neural_network.neurons[i], inputs);
                if (distance < min_distance)
                {
                    bmu_index = i;
                    min_distance = distance;
                }
            }
            return bmu_index;
        }

        private static void teach_network(ref NeuralNetwork neural_network, List<List<double>> teaching_images)
        {
            for (int epoch = 0; epoch < count_of_epochs; epoch++)
            {
                for (int image_number = 0; image_number < count_of_learning_images; image_number++)
                {
                    double min_distance = Double.MaxValue;
                    int bmu_index = 0;
                    for (int i = 0; i < neural_network.neurons.Count; i++)
                    {
                        double distance = calculate_distance(neural_network.neurons[i], teaching_images[image_number]);
                        if (distance < min_distance)
                        {
                            bmu_index = i;
                            min_distance = distance;
                        }
                    }

                    for (int neuron_index = 0; neuron_index < neural_network.neurons.Count; neuron_index++)
                    {
                        for (int weight_index = 0; weight_index < teaching_images[image_number].Count; weight_index++)
                        {
                            double hfunc = neighborhood_kernel(epoch, neural_network.neurons[bmu_index].weights[weight_index], neural_network.neurons[neuron_index].weights[weight_index]);
                            double learning_rate = calculate_learning_rate(epoch);
                            neural_network.neurons[neuron_index].weights[weight_index] = neural_network.neurons[neuron_index].weights[weight_index] + hfunc * learning_rate * (teaching_images[neuron_index][weight_index] - neural_network.neurons[neuron_index].weights[weight_index]);
                        }
                    }
                    for (int neuron_index = 0; neuron_index < count_of_learning_images; neuron_index++)
                    {
                        Console.WriteLine("Distance from neuron number " + neuron_index + " to learning image number " + neuron_index + " at epoch number " + epoch + " : " + calculate_distance(neural_network.neurons[neuron_index], teaching_images[neuron_index]));
                    }
                }
            }
        }

        private static double neighborhood_kernel(int epoch, double winner_coordinate, double coordinate)
        {
            double dist = difference(winner_coordinate, coordinate);
            return Math.Exp(-dist * dist / (2 * calculate_sigma(epoch) * calculate_sigma(epoch)));
        }

        private static double calculate_sigma(int epoch)
        {
            return 1 * Math.Exp(-epoch / 5);
        }

        private static double calculate_learning_rate(int epoch)
        {
            return 0.1 * Math.Exp(-epoch / 1000);
        }

        private static double calculate_distance(Neuron neuron, List<double> unputs)
        {
            double sum = 0;
            for (int i = 0; i < unputs.Count; i++)
            {
                sum += (unputs[i] - neuron.weights[i]) * (unputs[i] - neuron.weights[i]);
            }
            return Math.Sqrt(sum);
        }

        private static double difference(double a, double b)
        {
            return Math.Abs(a - b);
        }

        static void Main()
        {
            run();
        }
    }
}
