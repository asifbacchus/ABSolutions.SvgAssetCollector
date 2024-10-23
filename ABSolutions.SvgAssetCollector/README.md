# SvgAssetCollector: Class Library

This service reads an SVG file either from the local file system or a remote URL using an injected `HttpClient`. The SVG
file is then parsed, the main node is extracted and additional attributes are added as configured. If there are any
issues during retrieval or parsing, a default SVG is returned.

This version uses an **in-memory** cache to reduce the calls to the upstream source. The cache can be disabled as
outlined in the configuration documentation.

**Please refer to the documentation site below for more information**

|               |                                                                     |
|---------------|---------------------------------------------------------------------|
| Git repo      | https://github.com/asifbacchus/ABSolutions.SvgAssetCollector        |
| Author        | A-B Solutions (Asif Bacchus)                                        |
| Contact       | asif@a-b.solutions                                                  |
| Documentation | https://a-b.solutions/documentation/nuget/SvgAssetCollector-Library |
| Nuget         | https://www.nuget.org/packages/ABSolutions.SvgAssetCollector        |
