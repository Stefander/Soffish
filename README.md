Soffish
=======

**SUMMARY**

*Soffish* is a Halo 4 map file reader, built in WPF and C#. This application was mainly built as a research application to revive my knowledge 
of the Blam map file format. 

**PURPOSE**

The application basically lets you open a Halo 4 map file (*.map). 
It then reads the file header and creates an offset list of all the objects in that map. It shows you some of the properties of the map,
such as the map magic (used to calculate offsets in the map file) and the tag index offset. This information can then be used to, for example, 
study the tag structure.
