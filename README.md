# Fan Platform
## Overview
The Fan Platform is a social platform designed for fans to share their creations, discover new ideas, and interact with other users. It allows users to upload pictures and descriptions of their creations, rate others' creations based on creativity and uniqueness, and search for inspiration by element name.

## Features
### Users can register, login, and manage their profiles.
Authentication is handled using JWT tokens, and authorization is enforced to restrict access to certain endpoints.

### Creation Management:
Users can upload pictures and descriptions of their creations.
Creations are stored in the database along with metadata such as name, description, picture url, and creator information.

### Scoring System:
Users can rate creations based on creativity and uniqueness.
Each user can score a creation only once.

### Search Functionality:
Users can search for creations by name, allowing them to discover specific creations or find inspiration based on element names.

## Technologies Used
ASP.NET Core: Backend framework for building web applications.
Entity Framework Core: Object-relational mapping (ORM) framework for interacting with the database.
JWT Authentication: JSON Web Tokens for securing API endpoints.
Microsoft SQL Server: Relational database management system for storing application data.
