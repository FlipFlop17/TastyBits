# Project Title

Brief project description goes here.

## Table of Contents

- [About](#about)
- [Installation](#installation)
- [Features](#features)
- [License](#license)

## About

**TastyBits is a web application created for the sole purpose of practicing Blazor functionalities. This project represents my first venture into web development, and as such, some parts of the page may appear unfinished or incomplete.**

TastyBits is a web application that allows users to manage their meal plans effectively. The application consists of a landing page that provides an overview of the features and functionalities. It primarily functions as a static page, providing information about the purpose and usage of the platform.

Upon registration and login, users gain access to their personalized dashboard, where they can manage their meals efficiently. The dashboard enables users to add new meals to their plan, view the meals already present in the database, and edit existing meals as needed. With a user-friendly interface, TastyBits aims to simplify meal management and provide a seamless experience for its users.

You can try the application at: 

Scroll to the bottom of the page and opt for the Demo version if you just want to try it out without registering.

![Landing](items/TastyLanding.png)

## Installation

1. Clone the project.
2. Inside Program.cs enable InMemoryDbUsage and disable options.UseNpgsql(conString)  
`
...  
 //options.UseNpgsql(conString);  
options.UseInMemoryDatabase("FlopsInMemory");  
options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));  
});
...  
` 
- If you want to create your own database just use migrations history to set everything up.


## Features

Adding new meals to the database, updating and deleting meals.

## License

MIT license

## Support

You can email me at petarsoce@gmail.com
