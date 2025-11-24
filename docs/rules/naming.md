## 📏 Naming Conventions & Folder Structure

To maintain a balance between **Searchability** (finding files quickly) and **Readability** (clean import paths), we follow the **"Global Files, Local Folders"** pattern.

### 1. The Core Rule

- **Folders** describe **Local Context** (Where am I?). They should be short and simple.
- **Files** describe **Global Identity** (What is this?). They must include a unique feature prefix.

### 2. File Naming Formula

Files must follow this format:
`[feature-prefix]-[context]-[type].ts`

| Part               | Description                          | Example         |
| :----------------- | :----------------------------------- | :-------------- |
| **Feature Prefix** | A unique short code for the feature. | `wizard-`       |
| **Context**        | What the component actually does.    | `template-list` |
| **Type**           | The Angular artifact type.           | `.component`    |

**Result:** `wizard-template-list.component.ts`

### 3. Folder Structure Example

Do not repeat the feature prefix in the folder name. Redundancy makes the file tree hard to read.

**✅ DO THIS (Recommended):**

```text
project-creation-wizard/           <-- Feature Root
├── components/
│   ├── template-list/             <-- Folder: Short (Local Context)
│   │   └── wizard-template-list.component.ts  <-- File: Unique (Global Context)
```
