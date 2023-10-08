Instruções (Para linux):

sudo docker ps -aqf "name=database_leitour2"
## resultado: container-id
sudo docker cp init.sql [container-id]:/init.sql
sudo docker ps
sudo docker exec -it [container-id] /bin/bash
mysql -u root -p
## try:
##    source init.sql
## if it doesn't work:
##    copy and paste the init.sql file content into the mysql terminal
clear; sudo docker compose up --build --remove-orphans