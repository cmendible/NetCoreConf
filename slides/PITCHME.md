---?image=slides/img/cover.png
@transition[none]

---?image=slides/img/sponsors.png
@transition[none]

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](I will not talk about)

* Docker
* Kubernetes
* Micro-services
* DevOps
* Politics

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Decoupling Applications)

* Your application starts simple
* One or may be two data sources
* One backend
* We all know what a Monolith is right?
* Who avoids premature optimization?

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Decoupling Applications)

* What if you need more data sources?
* What if you need different backend process?
* What happens when your system fails?
* What about the scalabiliy of both the system and your team?

Note:

* Initial decisions must change
* Experimentation
* Move to streaminmg architecture

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Decoupling Applications)

* Adding a new system to you monolith may be painfull
* Change is difficult cause applications are thigtly copupled
* Lack of velocity
* The more it grows the mora fragil the system becomes.
* Innovation is risky.

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Event Driven Communications)

* Event Bus
* Pub / Sub
* Implementations:
  * Azure Service Bus
  * Azure Event Hubs
  * Kafka
  * RabbitMQ
  * ...

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Events & Messages)

* Event: lightweight notification of a condition or a state change
* Message: A message is raw data produced by a service to be consumed or stored elsewhere.

Note:

* Event: lightweight notification of a condition or a state change. The publisher of the event has no expectation about how the event is handled. The consumer only needs to know that something happened.
* Message: A message is raw data produced by a service to be consumed or stored elsewhere. A contract exists between the two sides

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Comparision of Services)

@size[14px](
| Service | Purpose | Type | When to use |
| ------- | ------- | ---- | ----------- |
| Event Grid | Reactive programming | Event distribution (discrete) | React to status changes |
| Event Hubs | Big data pipeline | Event streaming (series) | Telemetry and distributed data streaming |
| Service Bus | High-value enterprise messaging | Message | Order processing and financial transactions |
)

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Kafka-enabled Event Hubs)

* TODO

Note:

* [Check](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-for-kafka-ecosystem-overview)

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Eventual Consistency)

* TODO

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Demo time!!!)

@snap[midpoint]
![BSOD](slides/img/bsod.png)
@snapend

---?image=slides/img/sponsors.png
@transition[none]

---?image=slides/img/end.PNG
@transition[none]