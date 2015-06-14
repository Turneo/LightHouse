# LightHouse

As an extensible business application framework LightHouse can be extended by creating custom extensions. 

A basic LightHouse business application requires as a minimum an Application, Model, Execution and Storage extension. Normaly it would be extended with a Presentation extension and in more complex scenarios enriched by a Charting, Security or Testing extension.

Out of the box, LightHouse offers the following extensions, that can customized further as required.

## Application

- Console
- Server
- Desktop WPF (DevExpress, not available as open source)
- Web Site MVC 
- Web App MVC (DevExpress, not available as open source)
- Mobile (Xamarin Forms, not available as open source)

## Model

Just default implementation for now.

## Execution

- Embedded

And the following providers: Embedded, Remote and Server.

## Storage

- Embedded

Ant the following repositories: AAD, Json, Memory, MySQL, RavenDB, Remote, SQLServer, TFS and Xml.

## Presentation

- Desktop WPF (DevExpress, not available as open source)
- Web App MVC (DevExpress, not available as open source)
- Mobile (Xamarin Forms, not available as open source)

## Charting

- Desktop WPF (DevExpress, not available as open source)
- Web App MVC (DevExpress, not available as open source)

## Security

- Embedded
- Web

And the following providers: AAD (Web & Desktop), AD (Desktop).

## Testing

- Embedded with xUnit Support (Selected to be able to run the test on portable devices).

LightHouse is a registered trademark of Turneo AG (www.turneo.com).
