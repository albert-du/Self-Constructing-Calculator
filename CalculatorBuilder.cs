using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;

class CalculatorBuilder
{
	static void Main(string[] Args)
	{
		if (Args.Length == 0)
		{
			Console.Clear();
			Console.WriteLine("Building Calculator");
			GenerateData();
			AssembleCalculator();
			return;
		}
		foreach (string arg in Args)
		{
			if (arg.Contains("-s")) { GeneratorSize = Int32.Parse(arg.Replace("-s","")); Main(new string[] {}); }
			if (arg == "-g") GenerateData(); 
			if (arg == "-a") AssembleCalculator(); 
		}
	}

	private static int GeneratorSize = 1000;
	private static void GenerateData()
	{ 
		int Operations = 0;
		List<string> OPERATORS = new List<string>() {"+","-","*","/"};

		for (int i = 0; i < 7; i++)	using (StreamWriter sw = File.CreateText($"{i.ToString()}.txt"));
		
		Console.WriteLine("Generating\n0%");

		string Answer = "";
		foreach(string Operator in OPERATORS)
		{
			for (int num1 = 0; num1 <= GeneratorSize; num1++)
			{
				for (int num2 = 0; num2 <= GeneratorSize; num2++)
				{
					if (Operator == "+") Answer = (num1+num2).ToString();
					else if (Operator == "-") Answer = (num1-num2).ToString();
					else if (Operator == "*") Answer = (num1*num2).ToString();
					else if (Operator == "/")
					{
						try
						{
							Answer = ( (float) num1/ (float) num2).ToString();
						}
						catch (DivideByZeroException)
						{
							Answer = "Nice Try";
						}
					}
					Operations++;
					using (StreamWriter sw = File.AppendText($"{(Operations%7).ToString()}.txt"))
					{
						sw.WriteLine($"if num1 == {num1.ToString()} and sign == \"{Operator}\" and num2 == {num2.ToString()}: print(\"{num1.ToString()} {Operator} {num2.ToString()} = {Answer}\")"); 
					}
					Console.SetCursorPosition(0, Console.CursorTop - 1);
					Console.WriteLine(LimitMax(((Operations/ (float) (GeneratorSize*GeneratorSize*4))*100f),100f).ToString() + "%                 ");
				}
			}
		}
	}
	private static void AssembleCalculator()
	{
		List<string> CalculatorAssembler = new List<string>() 
		{
			"#!/usr/bin/env python3",
			"import subprocess, sys, os",
			"def cleanup():",
			"    #os.remove(\"CalculatorBuilder.cs\")",
			"    #os.remove(\"CalculatorBuilder.exe\")",
			"    for _ in range(0,7):",
			"        try:",
			"            os.remove(f\"{str(_)}.txt\")",
			"        except: pass",
			"    subprocess.Popen(\"python -c \\\"import os, time; time.sleep(1); os.remove('{}');\\\"\".format(sys.argv[0]),shell=True)",
			"    subprocess.call('clear' if os.name =='posix' else 'cls')",
			"    print(\"Calculator Ready\\npress return to continue\")",
			"    sys.exit()",
			"data = []",
			"try:",
			"    for _ in range(0,7):",
			"        with open(f\"{str(_)}.txt\") as f:",
			"            data.append(f.readlines())",
			"except: cleanup()",
			"j = [1,2,3,4,5,6,0]",
			"i = 0","sorted_data = []",
			"active = True",
			"while active:",
			"    for _ in j:",
			"        active = not all(k < i for k in [len(data[0]),len(data[2]),len(data[3]),len(data[4]),len(data[5]),len(data[6])])",
			"        if len(data[_])-1 < i: continue", 
			"        sorted_data.append(data[_][i])", 
			"    i+=1", 
			"with open(\"Calculator.py\",\"w\") as f:",
			"    f.write(\"print(\\\"Calculator\\\")\\n\")",
			"    f.write(\"num1 = int(input(\\\"Choose your first number: \\\"))\\n\")",
			"    f.write(\"sign = input(\\\"Choose your operation +, -, /, or *: \\\")\\n\")",
			"    f.write(\"num2 = int(input(\\\"Choose your second number: \\\"))\\n\")", 
			"    f.writelines(sorted_data)",
			"    f.write(\"print(\\\"Thank you for using this calculator\\\")\\n\")",
			"cleanup()"
		};
		using (StreamWriter sw = File.CreateText("CalculatorAssembler.py"))
		{
			foreach(string line in CalculatorAssembler)
			{
				sw.WriteLine(line);
			}
		}
		using (Process Assembler = new Process())
		{
			Assembler.StartInfo.UseShellExecute = false;
			
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Assembler.StartInfo.FileName = "py";
			else Assembler.StartInfo.FileName = "python3";

			Assembler.StartInfo.Arguments = "CalculatorAssembler.py";
			Assembler.StartInfo.CreateNoWindow = true;
			Assembler.Start();
		}
		Console.Clear();
		Console.WriteLine("Building Calculator");
		Console.WriteLine("Assembling");
	}
	private static float LimitMax(float value, float MaxValue)
	{
		if (value <= MaxValue) return value;
		else return MaxValue;
	}
}