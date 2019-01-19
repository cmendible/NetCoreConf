---?image=slides/img/cover.png
@title[Cover]

---?image=slides/img/sponsors.png
@title[Sponsors]

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](I will not talk about)

- Docker
- Kubernetes
- Micro-services
- DevOps
- Politics

Note:

Esta claro que cualquiera de los temas que muestro en esta slide son topicos interesantes, pero nno os voy a dar la chapa con docker, k8s, DevOps o politica.

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Decoupling Applications)

- Your application starts simple
- One or may be two data sources
- One backend
- We all know what a Monolith is right?
- Who avoids premature optimization?

Note:

- Que es lo tipico cuando creamos aplicaciones? Pues que comenzamos con algo simple, quizas un portal con una quizas dos fuentes de datos.
- Lo tipico es tener un monolito entre manos
- Y que me dicen de optimizar temprano en el ciclo de vida de una aplicacion?

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Decoupling Applications)

- What if you need more data sources?
- What if you need different backend process?
- What happens when your system fails?
- What about the scalabiliy of both the system and your team?

Note:

- Las decisiones inciales las debemos cambiar
- Que pasa si queremos experimentar?
- Y si necesitamos movernos a una arquitectura de streaminmg?

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Decoupling Applications)

- Adding a new system to you monolith may be painfull
- Change is difficult cause applications are thigtly coupled
- Lack of velocity
- The more it grows the mora fragil the system becomes.
- Innovation is risky.

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Event Driven Communications)

- Event Bus
- Pub / Sub
- Implementations:
  - Azure Service Bus
  - Azure Event Hubs
  - Kafka
  - RabbitMQ
  - ...

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Events & Messages)

- Event: lightweight notification of a condition or a state change
- Message: A message is raw data produced by a service to be consumed or stored elsewhere.

Note:

- Event: lightweight notification of a condition or a state change. The publisher of the event has no expectation about how the event is handled. The consumer only needs to know that something happened.
- Message: A message is raw data produced by a service to be consumed or stored elsewhere. A contract exists between the two sides.

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Comparision of Services)

| Service | Purpose | Type | When to use |
| ------- | ------- | ---- | ----------- |
| Event Grid | Reactive programming | Event distribution (discrete) | React to status changes |
| Event Hubs | Big data pipeline | Event streaming (series) | Telemetry and distributed data streaming |
| Service Bus | High-value enterprise messaging | Message | Order processing and financial transactions |

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Kafka-enabled Event Hubs)

| Kafka Concept | Event Hubs Concept |
| ------------- | ------------------ |
| Cluster | Namespace |
| Topic | Event Hubs |
| Partition | Partition |
| Consumer Group |Consumer Group |
| Offset | Offset |

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Kafka Features (not supported))

- Idempotent producer
- Transaction
- Compression
- Size-based retention
- Log compaction
- Adding partitions to an existing topic
- HTTP Kafka API support
- Kafka Streams

Note:

- [Check](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-for-kafka-ecosystem-overview)

---?image=slides/img/slide.PNG

### @color[rgb(104, 32, 121)](Eventual Consistency)

> A consistency model used in distributed computing to achieve high availability that informally guarantees that, if no new updates are made to a given data item, eventually all accesses to that item will return the last updated value.

---?image=slides/img/bsod.png
@title[Joke]

---?image=slides/img/itcrowd.gif&size=contain
@title[Demo Time]

---?image=slides/img/sponsors.png
@title[Sponsors]

---?image=slides/img/end.PNG
@title[Thanks]