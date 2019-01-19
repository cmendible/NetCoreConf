---?image=slides/img/cover.png
@transition[none]

---?image=slides/img/sponsors.png
@transition[none]

---?image=slides/img/slide.png

### I will not talk about

* Docker
* Kubernetes
* Micro-services
* DevOps
* Politics

---

### Decoupling Applications

* Your application starts simple
* One or may be two data sources
* One backend
* We all know what a Monolith is right?
* Who avoids premature optimization?

---

### Decoupling Applications

* What if you need more data sources?
* What if you need different backend process?
* What happens when your system fails?
* What about the scalabiliy of both the system and your team?

Note:

* Initial decisions must change
* Experimentation
* Move to streaminmg architecture

---

### Decoupling Applications

* Adding a new system to you monolith may be painfull
* Change is difficulkt cause applications are thigtly copupled
* Lack of velocity
* The more it grows the mora fragil the system becomes.
* Innovation is risky.

---

### Event Driven Communications

* Event Bus
* Pub / Sub
* Implementations:
  * Azure Service Bus
  * Azure Event Hubs
  * Kafka
  * RabbitMQ
  * ...

---

### Events & Messages

* Event: lightweight notification of a condition or a state change
* Message: A message is raw data produced by a service to be consumed or stored elsewhere.

Note:

* Event: lightweight notification of a condition or a state change. The publisher of the event has no expectation about how the event is handled. The consumer only needs to know that something happened.
* Message: A message is raw data produced by a service to be consumed or stored elsewhere. A contract exists between the two sides

---

### Comparision of Services

| Service | Purpose | Type | When to use |
| ------- | ------- | ---- | ----------- |
| Event Grid | Reactive programming | Event distribution (discrete) | React to status changes |
| Event Hubs | Big data pipeline | Event streaming (series) | Telemetry and distributed data streaming |
| Service Bus | High-value enterprise messaging | Message | Order processing and financial transactions |

---

### Kafka-enabled Event Hubs

* TODO

Note:

* [Check](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-for-kafka-ecosystem-overview)

---

### Eventual Consistency

* TODO

---?color=#e7ad52

@snap[midpoint]
### Demo Time!
@snapend

---?image=slides/img/sponsors.png
@transition[none]

---?image=slides/img/end.png
@transition[none]