# Blazor WebApp based MinesWeeper game

This one is serverside hostet.

## Docker

The highscore values are stored in MariaDB database per MinesWeeperApi.
Needed environment variables:
- MINES_WEEPER_API_USER : username for authenticate to MinesWeeperApi
- MINES_WEEPER_API_PASSWORD : user password for authenticate to MinesWeeperApi
- MINESWEEPER_API_URL : URL to MinesWeeperApi host
- USERSERVICE_API_URL : URL to UserServiceApi to authenticate MinesWeeperApi credentials