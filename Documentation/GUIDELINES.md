
#LightHouse (Guideline)

### Class Guideline

Each Class, Property, Field and Method requires a comment.

Each comment should start upper case and end with an ".".

### Usings Guideline

The using namespace list should order like this.

1. System Usings
2. External Library Usings
3. LightHouse Usings
4. Turneo Product Usings (TITAN for example)
5. Product/Project Usings

Here an example:

```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;

using LightHouse.Core.Collections;
using LightHouse.Core.Views.Value.List;
using LightHouse.Model.Entities;
using LightHouse.Presentation.Appearance.List;
using LightHouse.Presentation.DXpress;
using LightHouse.Presentation.List;
using LightHouse.Presentation.Web.App.Controls;
using LightHouse.Storage.Linq.Extensions;
```
