# Serilog-Kibana8.Lab

<small>Version: 2404121</small>

<small>Author:</small>

- Thomas Grossi 
- Nashville, TN 
- TheAutomaTom@gmail.com 
- https://www.SurrealityCheck.org

## Environment setup

- Launch `Docker for Windows`.
- ~~Open an command prompt at the directory containing `compose.yaml`.~~
- ~~Run `docker compose up -d`.~~

- After a lot of experimentation, I found using this repo to set up ELK to be much more valuable than creating my own local-dev docker compose files: https://github.com/deviantony/docker-elk

## Observing the ELK stack operate

- Kibana 
	- Verify connectivity
		- Go to `http://localhost:5601/app/home/`		- Click the `☰` hamburger button in the top left, then select `Management/ Stack Management`
		- Select `Index Management`
		- Observe your created logging index with configured primary shards and replicas
		- Note the sane of your index for the next step.
	- Configure Logging Stream
		- Click the `☰` button in the top left, then select `Analytics/ Discover`		
		- On the second-from-top toolbar, under the `☰` button, click `V` carrot to open the dropdown.
		- Select `Create data view``
			- Set `Name` as appropriate
			- Set `Index pattern` to `$"{name-of-your-index-before-date}-*"` __(`*` is a wildcard*)__
						-	Example: `elk8-lab-api-*`
			- Select `Save Data View`
	- For an example of searching for a specific error
		- In Swagger, execute `ElasticsearchClient8/IntentionallyThrow?someParameter=666`
		- In Kibana, search for `IntentionallyThrow`
		- Observe the results!

## Technical References

- https://www.elastic.co/guide/en/kibana/current/data-views.html

- https://www.youtube.com/watch?v=zp6A5QCW_II&ab_channel=MohamadLawand

