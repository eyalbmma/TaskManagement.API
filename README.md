# TaskManagement.API

מערכת לניהול משימות מבוססת ASP.NET Core, SQL Server ו־Entity Framework Core.  
המערכת תומכת במשימות מסוג רכש (Procurement) ופיתוח (Development), וניתנת להרחבה בקלות.

---

## הוראות התקנה והרצה

### 1. דרישות מוקדמות

- .NET 8 SDK  
- SQL Server מותקן (לוקאלי או Azure)
- Visual Studio או VS Code

---

### 2. יצירת מסד נתונים והכנסת משתמשים

הרץ את הפקודה הבאה כדי לבנות את מסד הנתונים:

```bash
dotnet ef database update
```

לאחר מכן הכנס משתמשים לדוגמה למסד הנתונים:

```sql
USE TaskDb;

INSERT INTO Users (FullName) VALUES ('Alice Developer');
INSERT INTO Users (FullName) VALUES ('Bob Procurement');
INSERT INTO Users (FullName) VALUES ('Charlie QA');
```

---

### 3. הרצת השרת

```bash
dotnet run
```

ברירת מחדל להרצה:  
https://localhost:7099

---

## API מרכזיים

### משימות (Tasks)

- `POST /api/tasks`  
  יצירת משימה חדשה  
  גוף הבקשה:
  ```json
  {
    "taskType": "Development",
    "assignedUserId": 1
  }
  ```

- `POST /api/tasks/{taskId}/change-status?nextStatus=2&newUserId=1`  
  מעבר קדימה או אחורה במצב המשימה

- `POST /api/tasks/{taskId}/close`  
  סגירת משימה (רק מהשלב הסופי)

### משתמשים (Users)

- `GET /api/users`  
  שליפת כל המשתמשים

- `GET /api/users/{userId}/tasks/simple`  
  שליפת כל המשימות של משתמש מסוים

---

## מבנה הפרויקט

```
TaskManagement.API/
├── Controllers/
│   ├── MvcTasksController.cs
│   ├── TasksController.cs
│   └── UsersController.cs
├── DTOs/
├── Data/
├── Models/
├── Migrations/
├── Middlewares/
├── Services/
├── Program.cs
├── README.md
```

---

## חוקי עבודה כלליים במערכת

1. כל משימה משויכת למשתמש אחד בלבד  
2. משימה יכולה להיות פתוחה או סגורה  
3. התקדמות בסטטוס מותרת רק בשלבים עוקבים (אין דילוגים)  
4. חזרה לאחור מותרת בכל שלב  
5. סגירה מותרת רק מהסטטוס הסופי  
6. משימה סגורה לא ניתנת לעריכה  
7. כל שינוי סטטוס מחייב:
   - אימות שדות חובה לפי סוג המשימה
   - הקצאת משתמש הבא

---

## תמיכה בסוגי משימות

### Procurement (רכש)

| סטטוס | דרישת נתונים   |
|--------|----------------|
| 2      | Offer1 + Offer2 |
| 3      | Receipt         |
| סגירה | רק מסטטוס 3     |

### Development (פיתוח)

| סטטוס | דרישת נתונים   |
|--------|----------------|
| 2      | Specification   |
| 3      | BranchName      |
| 4      | Version         |
| סגירה | רק מסטטוס 4     |

---

## בדיקות

ניתן לבדוק את המערכת בעזרת:

- Postman / Insomnia
- Swagger UI (אם מופעל)
- פקודות curl מהטרמינל

---

## הערות

- הארכיטקטורה תומכת בהוספת סוגי משימות חדשים מבלי לגעת בקוד הקיים  
- הקוד בנוי לפי עקרונות SRP ו־SOC  
- המערכת כוללת גם תצוגת MVC וגם קליינט React (בתיקיה נפרדת)

