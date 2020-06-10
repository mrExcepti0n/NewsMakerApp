# DataSearchAngularApp

Новостной портал написанный на Angular + .net core с использованием docker. 

Присутствует возможность добавлять и редактироваться посты.

Добавлен модуль комментирования, взаимодействие клиентского приложения с сервером осуществляется через SignalR(WebSockets).

Для гибкого взаимодействия интеграционных процессов используется брокер сообщений RabbitMQ (RabbitMQClient) и библиотека MediatR.

Для быстрого поиска новостной информации используется ElasticSearch.

Для доккументации используется swagger и библиотека swashbuckle.

Для логирования используется serilog + elasticsearch + kibina.

Простая диаграмма с группировкой по типу сообщений:
![диаграмма](https://github.com/mrExcepti0n/NewsMakerApp/raw/master/.images/kibana-diagram.png)
