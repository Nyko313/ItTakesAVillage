using System;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class FFTHelper
    {
        private StreamWriter writer;
        public double[] values;
        private double[] orderedValues;
        private bool orderedValuesDirty = true;
        public double[] fft;
        public int currentIndex = 0;
        public bool isFilled = false;

        private bool logData = false;
        public FFTHelper(int size, bool logData = false)
        {
            
            values = new double[size];
            orderedValues = new double[size];
            fft = new double[size];
            this.logData = logData;
        }
        
        public double[] getDataInOrder()
        {
            if (!isFilled)
            {
                return Array.Empty<double>();
            }

            if (orderedValuesDirty)
            {
                // Debug.Log($"First part: source: {currentIndex} Destination: {0} Length: {values.Length - currentIndex}");
                Array.Copy(values, currentIndex, orderedValues, 0, values.Length - currentIndex);
                // Debug.Log($"Second part: source: {0} Destination: {currentIndex} Length: {currentIndex}");
                Array.Copy(values, 0, orderedValues, values.Length - currentIndex, currentIndex);
                orderedValuesDirty = false;
            }

            return orderedValues;
        }

        public void insertValue(float current)
        {
            values[currentIndex] = current;
            currentIndex += 1;
            orderedValuesDirty = true;
            if (currentIndex >= values.Length)
            {
                isFilled = true;
                currentIndex = 0;
            }
        }

        public bool analyse(float threshold, int count, string prefix = "")
        {
            if (this.getDataInOrder().Length == 0)
            {
                return false;
            }

            // Calculate the FFT as an array of complex numbers
            System.Numerics.Complex[] spectrum = FftSharp.FFT.ForwardReal(this.getDataInOrder());
            int containsValue = spectrum.Count(x => Math.Abs(x.Real) > threshold);

            if (logData)
            {
                // Debug.Log($"{prefix} ContainsValue {containsValue}");
                // Debug.Log($"{prefix} {spectrum.Select(x => x.Real).ToSeparatedString(";")}");

                string filename = $"./{prefix}_data.csv";
                if (File.Exists(filename))
                {
                    File.WriteAllText(filename, string.Empty);
                }

                writer = new StreamWriter(filename);
                writer.WriteLine($"TIME,{prefix}ACC");
                for (int i = 0; i < spectrum.Length; i++)
                {
                    writer.WriteLine($"{i},{spectrum[i].Real}");
                }

                writer.Flush();
                writer.Close();
            }

            return containsValue > count;
        }
    }
}