using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decay
{
    class Program
    {
        //Input arguments
        static DecayModel _selectedModel;
        static int _maxDecompositionClass;
        static int _maxTimeStep;
        static string _fileNameInitialValues = "";
        static string _fileNameMortality = "";
        static string _fileNameOutput = "";

        static void Main(string[] args)
        {
            //Checks that the program recieved correct number of arguments (input parameters)
            if (args.Count() == 6 || args.Count() == 7)
            {
                if (DecayModel.TryParse(args[0], out _selectedModel) &&
                    Int32.TryParse(args[1], out _maxDecompositionClass) &&
                    Int32.TryParse(args[2], out _maxTimeStep))
                {
                    _fileNameInitialValues = args[3];
                    _fileNameMortality = args[4];
                    _fileNameOutput = args[5];
                }
            }
            else
            {
                Console.WriteLine("The program needs four input arguments! At least one of them is missing.");
                Console.ReadLine();
            }

            if (_fileNameOutput.Length > 0) //Is fullfilled at this point only if required arguments exist.
            {
                //Checks that input file with initial values exists. This file is not essential. If not existing then initial values are just ignored.
                //If simulation shoul be calculated without initial valus the "Ignored" is a recommended word to use as argument.
                if (!System.IO.File.Exists(_fileNameInitialValues))
                {
                    _fileNameInitialValues = "";
                }

                //Checks that input file with mortalities exists. If not existing then the simulations can not be completed.
                if (!System.IO.File.Exists(_fileNameMortality))
                {
                    Console.WriteLine("The inputfile named {_fileNameMortality} does not exist. This file is required.");
                    Console.ReadLine();
                    _fileNameMortality = "";
                }
            }
            
            
            if (_fileNameMortality.Length > 0) //Is fulfilled at this point only if the essential mortality input file exists.
            {
                Console.WriteLine("Starts processing decay simulations using the " + _selectedModel.ToString() + " model for output file: " + System.IO.Path.GetFileNameWithoutExtension(_fileNameOutput));
                
                SimulationManager simulationManager = new SimulationManager(_selectedModel, _maxDecompositionClass, _maxTimeStep);
                //Read input file with initial values if existing.
                if (_fileNameInitialValues.Length > 0)
                {
                    if (!simulationManager.SetInitialValues(_fileNameInitialValues))
                    {
                        
                        Console.WriteLine("The input file " + _fileNameInitialValues + " has errors. Simulations were interupted.");
                        Console.ReadLine();
                        _fileNameMortality = "";
                    }

                }
                else
                {
                    Console.WriteLine("Simulations proceed without any initial values.");
                }

                //Read input file with mortalities.
                if (_fileNameMortality.Length > 0) //Is fulfilled at this point if no errors have occurred reading initial values.
                {
                    if (!simulationManager.SimulateDecayOfAddedLogs(_fileNameMortality))
                    {
                        
                        Console.WriteLine("The input file " + _fileNameMortality + " has errors. simulations were interupted.");
                        Console.ReadLine();
                        _fileNameOutput = "";
                    }

                }

                //Save output file
                if (_fileNameOutput.Length > 0) //Is fullfilled if no errors has occurred at this point.
                {
                    if (simulationManager.SaveResults(_fileNameOutput))
                    {
                        Console.WriteLine("Output file was successfully saved");
                    }
                    else 
                    {
                        Console.WriteLine("It was not possible to save the output file.");
                    }
                }
                
                //If user has added a "Break" argument in the end of the line then the program breaks after the processing has ended until the user hits ENTER
                if (args.Count() == 7) Console.ReadLine();
            }
        }
    }
}
