### Scripts list ###
* add a docker/.wslconfig file to your C:\users\<yourUserName>\ folder and limit the amount of memory and cpu cores used there
* docker/setup-network.ps1 script is responsible for creation of network
* docker/setup-ssl-cert.ps1 script is responsible for creation of ssl certificate
* docker/setup-infrastructure.ps1 script is responsible for setting up infrastructure docker containers
* Run.ps1 script is responsible for setting up projects docker images

### How do I get set up? ###

* Run .docker/setup-network.ps1 script
* Run .docker/setup-ssl-cert.ps1 script
* Run .docker/setup-infrastructure.ps1 script
* Run Run.ps1 script

### How do I get set up? ###

To provide scalibility I used akka.net framework in calculation service.

Due to the request / response nature of HTTP, there are really only two communication patterns we can use between controllers and actors:

Request / Response - send a message to our actors; expect a response back.
One-way - fire off a message to an actor; move on immediately without expecting a response.

In this project I used first approach, but I would recommend second option. Response will be delivered thanks to SignalR like in case described in link below. 

https://petabridge.com/blog/akkadotnet-goes-to-wall-street/

I put some effort to dockerize everything. There should not be problem to run project.

PS. I would not say it is production ready. I've spent ~6h on this project, but working with akka is more consumig than standard approaches. 
Besides I was focused on deliverind POC that shows how it should be done. 
So there are places that requires refactorization. Those places are marked with comment TODO. 

