# Environment setup

- Launch `Docker for Windows`.
- Open an command prompt at the directory containing `compose.yaml`.
- Run `docker compose up -d`.
- Kibana 
	- Verify connectivity
		- Go to `http://localhost:5601/app/home/`		- Click the `hamburger` button in the top left, then select `Management/ Stack Management`
		- Select `Index Management`
		- Observe your created logging index with configured primary shards and replicas
		- Note the sane of your index for the next step.
	- Configure Logging Stream
		- Click the `hamburger` button in the top left, then select `Analytics/ Discover`		
		- Select `Create data view``
			- Set `Name` as appropriate
			- Set `Index pattern` to `$"{name-of-your-index-before-date}-*"` __(`*` is a wildcard*)__
						-	Example: `elk8-lab-api-*`
			- Select `Save Data View`
	- For an example of searching for a specific error
		- In Swagger, execute `ElasticsearchClient8/IntentionallyThrow?someParameter=666`
		- In Kibana, search for `IntentionallyThrow`
		- Observe the results!

# Technical References

- https://www.elastic.co/guide/en/kibana/current/data-views.html

- https://www.youtube.com/watch?v=zp6A5QCW_II&ab_channel=MohamadLawand

