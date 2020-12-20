'''
Created on 19 Dec 2020

@author: jackm
'''
from PIL import Image
import glob

icons = []
names = []

for filename in glob.glob(r'C:\Users\jackm\Downloads\Chess Icons\132 x 132\*.png'):
    names.append(filename.split("\\")[6])
    im=Image.open(filename)
    icons.append(im)

for i in range(0, 12):
    im = icons[i]
    n = names[i]
    
    im = im.resize((80, 80))
    im.save('C:\\Users\\jackm\\Downloads\\Chess Icons\\80 x 80\\' + n)

