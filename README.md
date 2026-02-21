# EnrichSystem

一个基于 **.NET 8 Web API + Vue 3** 的 `Project Reboot` 原型系统，用于管理双轨货币（铜币/琥珀币）、任务结算、日志与商店。

## 目录结构

- `backend/`：ASP.NET Core Minimal API
- `frontend/`：Vue + Vite 前端

## 快速启动

### 1) 启动后端

```bash
cd backend
dotnet run
```

默认地址：`http://localhost:5000`。

### 2) 启动前端

```bash
cd frontend
npm install
npm run dev
```

默认地址：`http://localhost:5173`，已代理 `/api` 到后端。

## 当前实现功能

- Dashboard：铜币/琥珀币余额、吉他基金、回国进度、阶段倍率。
- Quest Board：日常任务与悬赏任务，支持 RPG 风格描述与结算。
- Chronicle：自动流水日志 + 每日文字结项。
- Shop：可购买项目，扣除对应货币。
- 阶段推进：支持 P2/P3/P4 倍率切换。

> 注：目前为内存存储原型，重启后数据会重置。
