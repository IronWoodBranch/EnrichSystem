# EnrichSystem

一个基于 **.NET 8 Web API + Vue 3** 的 `Project Reboot` 原型系统。
当前前端界面稳定，后续重点放在后端规则演进与接口设计。

## 目录结构

- `backend/`：ASP.NET Core Minimal API（规则引擎 + API）
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

## 后端已实现规则（当前重点）

- 双轨货币：铜币 / 琥珀币独立结算。
- 阶段推进：P2/P3/P4 倍率（1.0/1.5/2.0）。
- 任务系统：日常任务与悬赏任务，悬赏任务一次性完成。
- 特赦令：22:00 后且学习 ≥5 分钟，可触发保底打卡奖励。
- 解锁树：
  - 电吉他需 `N1 + AWS SAA`
  - 吉他周边需 `体重<62.5 + 阅读完成`
- 吉他基金：支持按 `1:500` 从铜币兑换存入基金。
- 编年史：记录所有收支原因与日记型文字结项。

> 注：目前为内存存储原型，重启后数据会重置。
