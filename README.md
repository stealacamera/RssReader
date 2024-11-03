![](<Misc/showcase/intro.png>)
# RssReader
![.NET](https://img.shields.io/badge/.NET-512BD4?style=flat&logo=.net&logoColor=white) ![Angular](https://img.shields.io/badge/Angular-0F0F11?style=flat&logo=angular&logoColor=white) ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat&logo=postgresql&logoColor=white) ![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)

_.Net minimal API_ coupled with a simplistic _Angular frontend_ dedicated to susbcribing and reading RSS feeds

Features
- Folder- and tag-based organization of RSS feed subscriptions
- Folder-, tag-, RSS feed-specific filtering of the user's dashboard feed

## API
To explore the API endpoints, use the [Swagger editor](https://editor.swagger.io/) to read the [documentation](Misc/api-documentation.json).

Points of note regarding the API:
- API utilizes JWT authentication (alongside OTP verification) & Redis cache
- API continuously pulls all RSS feeds in the background in 30-second internals
- Users are subscribed to RSS feeds, rather than creating new ones, so as to cut down on data redundancy

![Database ERD schema](Misc/db-schema.png)

## Frontend
The webpage is a sample showcase of the base functionalities of the API, i.e.:
- Signing up & logging in
- Creating folders and subscriptions
- Adding new tags to a subscription
- Filtering dashboard feed

![Sign up page](Misc/showcase/signup-view.png "Sign up page") ![Login page](Misc/showcase/login-view.png "Login page")
![Email verification page](Misc/showcase/emailotp-view.png "Email verification page") 
![Dashboard page](Misc/showcase/homepage-view.png "Dashboard page") ![Feed item view](Misc/showcase/feeditem-view.png "Feed item view")
![Showcase of adding a new folder and feed subscription](Misc/showcase/addfeed-action.gif "Showcase of adding a new folder and feed subscription")
