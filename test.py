#!/usr/bin/env python3
import sys

def cleanup():
	#os.remove("CalculatorBuilder.cs")
	#os.remove("CalculatorBuilder.exe")
	for _ in range(0,7):
		os.remove(f"{str(_)}.txt")
	#subprocess.Popen("python -c \"import os, time; time.sleep(1); os.remove('{}');\"".format(sys.argv[0]),shell=True)
	sys.exit()
data = []
for _ in range(0,7):
	with open(f"{str(_)}.txt") as f:
		data.append(f.readlines())
#print(data)
j = [1,2,3,4,5,6,0]
i = 0
sorted_data = []
active = True
while active: 
	for _ in j:
		active = not all(k < i for k in [len(data[0]),len(data[2]),len(data[3]),len(data[4]),len(data[5]),len(data[6])])
		if len(data[_])-1 < i: continue
		sorted_data.append(data[_][i])
	i+=1
with open("final_data.txt","w") as f:
	f.writelines(sorted_data)
cleanup()


print(sys.version)

