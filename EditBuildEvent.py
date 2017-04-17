#!/usr/bin/python
# -*- coding: UTF-8 -*-
import os
import xml

namespace = {'owl': 'http://schemas.microsoft.com/developer/msbuild/2003'}
list = []

def findcsproj(dir):
    for file in os.listdir(dir):
        if(file.startswith(".")):
            continue
        file = os.path.join(dir,file)
        if(os.path.isdir(file)):
            findcsproj(file)
        else:
            if file.endswith(".csproj"):
                 list.append(file) 

def editcsprojUseElementTree(list):
    for csproj in list:
        print(csproj)
        from xml.etree import ElementTree as et
        tree = et.parse(csproj)
        str = tree.find(".//owl:PostBuildEvent",namespace)
        print "From:=============" + str.text
        result = str.text.replace("start /b xcopy","start /wait /b xcopy")
        print "To:===============" + result
        str.text = result
        tree.write(csproj)

def editcsprojUseDom(list)

findcsproj(".")
editcsproj(list)

    

            