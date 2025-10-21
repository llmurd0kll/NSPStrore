# 🛒 NSP Store

![Build](https://github.com/username/nsp-store/actions/workflows/docker-build.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

**NSP Store** — проект интернет‑магазина с полноценной пользовательской частью и административной панелью.  
Проект демонстрирует владение **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server**, а также современными практиками фронтенд‑разработки (**HTML5, CSS3, BEM, адаптивная верстка**).

---

## 🌐 Demo
👉 [Открыть NSP Store](https://nspstore.railway.app)  

---

## 🚀 Возможности

### 👤 Пользовательская часть
- Каталог товаров с фильтрацией и адаптивной сеткой.
- Страница товара с описанием, ценой и кнопкой «Добавить в корзину».
- Корзина с подсчетом итоговой суммы.
- Оформление заказа (checkout).
- Личный кабинет: профиль, список заказов.
- Авторизация и регистрация.

### 🛠 Административная панель
- Управление товарами (CRUD).
- Управление категориями.
- Управление пользователями.
- Управление заказами (смена статуса, просмотр деталей).
- Dashboard с KPI (товары, категории, пользователи, заказы, выручка) и аналитикой (последние заказы, топ‑товары).

---

## 🏗 Архитектура

Проект построен по многослойной архитектуре:

- **Domain** — сущности и бизнес‑логика.
- **Infrastructure** — доступ к данным (EF Core, миграции).
- **Application** — сервисы, валидация, бизнес‑правила.
- **Web** — ASP.NET Core MVC, Razor Views, контроллеры.

---

## 🗄 Схема базы данных

- **Users** ↔ **Orders** (1 ко многим).  
- **Orders** ↔ **OrderItems** (1 ко многим).  
- **Products** ↔ **Categories** (многие к одному).  
- **OrderItems** ↔ **Products** (многие к одному).  

---

## 🎨 UI‑kit и стили

Фронтенд построен на модульной системе CSS:

- `reset.css` — сброс стилей.
- `variables.css` — палитра, типографика, брейкпоинты.
- `layout.css` — контейнеры, сетка, навигация, заголовки.
- `components.css` — кнопки, формы, таблицы, бейджи.
- `product.css`, `cart.css`, `checkout.css`, `orders.css`, `profile.css`, `auth.css` — модули страниц.
- `admin-orders.css`, `admin-dashboard.css` — модули админки.
- `site.css` — главный файл, собирающий все модули.

Все стили написаны по методологии **BEM**, с акцентом на адаптивность и доступность.

---

## ⚙️ Технологии

- **Backend**: ASP.NET Core MVC, C#, Entity Framework Core, SQL Server.  
- **Frontend**: HTML5, CSS3 (Grid, Flexbox, BEM), адаптивная верстка.  
- **Auth**: ASP.NET Core Identity (роли: Admin, Manager, User).  
- **Документация**: комментарии в коде, README.md.  

---

## 📊 Dashboard (админка)

- KPI‑карточки: количество товаров, категорий, пользователей, заказов, выручка.  
- Последние заказы (с подсветкой новых).  
- Топ‑товары за последние 30 дней.  

---

## 🧑‍💻 Запуск проекта

### Локально
```bash
git clone https://github.com/username/nsp-store.git
cd nsp-store/NspStore
dotnet ef database update
dotnet run
```
### Через Docker
```bash
docker run -p 5000:80 ghcr.io/llmurd0kll/nspstore:latest
```

---

## ⚙️ CI/CD
- GitHub Actions: автоматическая сборка, тесты, публикация Docker‑образа в GHCR
- Railway: автодеплой при обновлении образа

---

## 🤝 Contributing
- Pull‑requests приветствуются! Для крупных изменений сначала открой issue.

---

## 📄 License
Этот проект распространяется под лицензией MIT.

---