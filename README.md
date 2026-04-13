# Tungsten Power PDF – Connector for Kofax TotalAgility

This repository provides a .NET Framework-based connector for integrating Tungsten Power PDF with Kofax TotalAgility. It allows users to send the currently open PDF document directly to TotalAgility, creating a job on a configured process with a single click from the Power PDF ribbon.

---

## 📁 Folder Structure

```
PowerPDF-TotalAgility-Connector/
├── PPDF.TotalAgility.Connector/        # Source project (.csproj)
├── PPDFTATester/                        # Standalone test harness (WinForms)
├── References/                          # External DLLs (DMSConnector.dll)
├── PowerPDF-TotalAgility-Connector.sln  # Visual Studio solution file
├── README.md
```

---

## 🛠 Build Instructions

1. Open `PowerPDF-TotalAgility-Connector.sln` in Visual Studio 2019 or later
2. In the project properties:
   - Set **Platform Target** to `AnyCPU` under Build
   - Uncheck **Prefer 32-bit**
   - Ensure `AssemblyInfo.cs` has a valid `AssemblyVersion`
3. Build the solution in **Release** mode

The output DLL will be located at:
```
PPDF.TotalAgility.Connector\bin\Release\PPDF.TotalAgility.Connector.dll
```

---

## 🚀 Deployment Steps

### 1. Copy the Connector DLL and Dependencies

Copy the following files to your Power PDF Connectors folder:

| File | Source |
|---|---|
| `PPDF.TotalAgility.Connector.dll` | `bin\Release\` |
| `Newtonsoft.Json.dll` | `bin\Release\` |

**Kofax Power PDF 51:**
```
C:\Program Files (x86)\Kofax\Power PDF 51\bin\Connectors\
```

**Tungsten Power PDF 2025:**
```
C:\Program Files\Tungsten\Power PDF 2025\bin\Connectors\
```

---

### 2. Register the Connector DLL

Run from an **elevated (Administrator) command prompt**:

**Kofax Power PDF 51 (32-bit):**
```
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe" "[Connectors folder]\PPDF.TotalAgility.Connector.dll" /codebase
```

**Tungsten Power PDF 2025 (64-bit):**
```
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe" "[Connectors folder]\PPDF.TotalAgility.Connector.dll" /codebase
```

To unregister:
```
regasm.exe /unregister "[Connectors folder]\PPDF.TotalAgility.Connector.dll"
```

---

### 3. Update Publish Mode Configuration

To make the connector button appear in the Connectors ribbon tab:

Open:
```
[Power PDF install folder]\Resource\PowerPDF\UILayout\Publish Mode.xml
```

Locate the line:
```xml
<toolbar name="connectors" shortKey="N">
```

Add the following immediately after that line:
```xml
<PFFGroup name="connector::TotalAgility" GroupType="PFFTitleBlock" autoZip="0">
    <PFFButton name="connector::tool::TotalAgility:menuItem0"/>
</PFFGroup>
```

---

### 4. Add Connector Display Name

Edit:
```
[Power PDF install folder]\Resource\PowerPDF\ENU\NameAndTitle.xml
```

Add under the `<!--connectors toolbar-->` section:
```xml
<PFFGroup name="connector::TotalAgility" title="Send to TotalAgility" />
```

---

### 5. Clear Cache and Restart

Delete the cached layout file if it exists:
```
%appdata%\Kofax\PDF\PowerPDF\UILayout\Publish.xml
```

Restart Power PDF to apply the updated UI.

---

## ⚙️ Configuration

After installation, configure the connector in Power PDF:

1. Go to **File → Options → Connectors → TotalAgility**
2. Enter your **TA SDK URL** (e.g. `http://server/TotalAgility/Services/Sdk`)
3. Enter your **User ID** and **Password**
4. Click **Connect** to authenticate and load available processes
5. Select a **Process** from the dropdown
6. Fill in any **Initialization Variables** shown
7. Click **Save**

---

## 🎯 Usage

1. Open a PDF document in Power PDF
2. Click **Send to TotalAgility** in the Connectors ribbon tab
3. The connector will authenticate, submit the PDF and create a job on the configured process
4. A result screen will show the **Job ID**, **Process Name**, **Document Name** and **Created At** timestamp

---

## 🧹 Troubleshooting

| Issue | Solution |
|---|---|
| Connector not appearing in ribbon | Verify `Publish Mode.xml` entry and delete cached `Publish.xml` |
| Connector not appearing in Options | Verify DLL is registered via regasm and registry key exists under `HKLM\Software\WOW6432Node\ScanSoft\Connectors\TotalAgility` |
| 64-bit registration error | Use `Framework64` regasm for Tungsten Power PDF 2025 |
| Job creation fails | Verify SDK URL, credentials and process configuration in Options |
| Newtonsoft.Json not found | Copy `Newtonsoft.Json.dll` to the Connectors folder |

---

## ✅ Deployment Checklist

| Task | Status |
|---|---|
| Build DLL in Visual Studio (AnyCPU, Release) | ✅ |
| Copy DLL and Newtonsoft.Json.dll to Connectors folder | ✅ |
| Register DLL via regasm (32-bit or 64-bit as appropriate) | ✅ |
| Update `Publish Mode.xml` | ✅ |
| Update `NameAndTitle.xml` | ✅ |
| Clear cached Publish.xml and restart Power PDF | ✅ |
| Configure connector (SDK URL, credentials, process) | ✅ |

---

## 📬 Support

For questions or contributions, please raise an issue or submit a pull request to this repository.
