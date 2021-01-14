# APCUPSD in VB.NET
Implementation of the APCUPSD NIS protocol for Visual Basic .NET

## Overview ##
An interrupted power supply (UPS) is a system that comprises of a battery, DC to AC power inverter, and usually protection circuitry. A UPS protects equipment by providing a short time of power backup anywhere from 5 minutes to a couple hours depending on the load.

APC by Schneider Electric is a renown company that provides an extensive line of UPS products. Some UPS devices do not have an LCD display and that means certain values and functions would not be possible unless you install additional software. APCUPSD, short for American Power Conversion uninterruptible power supply daemon, is a free utility that runs on Linux, UNIX, macOS and Windows. It allows the computer to interact with APC UPS devices. APCUPSD also works with some other OEM APC power supplies. APCUPSD has a TCP server with an NIS protocol that allows other computers on the same network to retrieve information about the UPS device.

This repository provides a VB.NET implementation to demonstrate the retrieval of system information from APCUPSD. It can provide a basis for developers who want to use APCUPSD in a computer lab with multiple computers or for use-cases where multiple UPS devices need to be monitored.

![APCUPSD App](https://raw.githubusercontent.com/dominicklee/APCUPSD-in-VB.NET/main/screenshot.png)

## Installation ##
1. [Download APCUPSD](http://www.apcupsd.org) for your OS. This is the computer in which you will be plugging in your UPS system. 
2. Install the APCUPSD driver and edit the `apcupsd.conf` file accordingly. Run the APCUPSD service.
3. Download or fork this repository. Open the the project files in Visual Studio 2008 or later. The code is in `Form1.vb`.
4. Run the program and type in the IP or hostname of your computer to fetch the info.

## References ##
[https://en.wikipedia.org/wiki/Apcupsd](https://en.wikipedia.org/wiki/Apcupsd)<br />
[http://www.apcupsd.org/manual/#nis-network-server-protocol](http://www.apcupsd.org/manual/#nis-network-server-protocol)<br />
[https://www.apc.com/us/en/](https://www.apc.com/us/en/)