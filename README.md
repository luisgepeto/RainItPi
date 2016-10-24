# RainItPi
A raspberry pi project for RainIt.
This project is designed to be run in a Raspberry PI with the latest Python 3.4 installed.

It consumes web services provided by an ASP.NET WebAPI which will return different binary matrix representation of images and patterns which  will later be rendered by some GPIO mechanism. The program makes use of asynchronous handling of operations in order to be able to enqueue the displayed patterns without any delay. 

In order to execute the program, the separate web api services need to be running on your local machine. The value needs to be updated inside the `rainit.config` file.

The structure of the project is as follows:

* **RainIt/rain_it/adapter** 
    - Contains adapters for several functionalities such as GPIO interaction and WebAPI request handlers.
* **RainIt/rain_it/builder** 
    - Contains builders that determine the different access mechanisms to the source information (image patterns). 
    - Builders determine how the information can be retrieved i.e. by a call to the WebAPI, or by reading serialized information
    - The director component indicates the type of element that will be retrieved 
* **RainIt/rain_it/interfaces** 
    - Contains interfaces that provide generic behavior to different elements
* **RainIt/rain_it/rain_it** 
    - Provides the main entry point of the program and several other helper functions for the main logic.
* **RainIt/rain_it/ric** 
    - Provides DTO's needed for the image pattern manipulation as well as other necessary configuration classes.
* **RainIt/rain_it/writer** 
    - Contains different classes that can write the output in different ways, i.e. to a file, GPIO or to the console.
    
    
    
