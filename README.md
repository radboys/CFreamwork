# CFramework - Unity Game Development Framework

> **Note**: This README serves as a comprehensive framework overview. Some features described here are currently in development or planned for future implementation. The framework is continuously evolving, and this documentation represents the complete vision of the system architecture.

## Overview
CFramework is a game development framework based on the Unity engine, designed with a modular architecture that provides a complete game development solution. The framework follows the principles of high cohesion and low coupling, separating core systems from functional modules to support rapid development and flexible expansion.

## Core Systems
### Global Manager
- Singleton pattern implementation for global unique instances
- Unified initialization process with debug mode support
- Automatic lifecycle management and application quit handling

### Update Dispatcher
- Multi-phase update support (Input/Logic/Physics/Render)
- Interface-based update mechanism for module decoupling
- Automatic registration and deregistration of update objects

### Event System
- Support for parameterized and non-parameterized event subscriptions
- Delegate-based event triggering mechanism
- Event cleanup and listener management

## Functional Modules
### Scene Management
- Support for synchronous/asynchronous scene loading
- Scene lifecycle management (enter/exit)
- Scene transition effects support

### Resource Management
- Support for both Resources and Addressables loading methods
- Asynchronous loading to prevent stuttering
- Unified resource loading interface

### Object Pool
- Automatic object reuse mechanism
- Dynamic capacity expansion support
- Object lifecycle management

### Audio Management
- Separate management of background music and sound effects
- Volume control support
- Configuration-based audio resource management

### UI System
- Layer-based management (Bottom/Middle/Top/System)
- Panel lifecycle management
- Automatic UI component finding and event binding

### Input Management
- Based on Unity's new Input System
- Multi-platform input mapping support
- Configurable input behaviors

### Game Flow
- Game state management
- Process control framework
- State transition and lifecycle management support

## Technical Features
### Performance Optimization
- Object pool to reduce GC pressure
- Asynchronous loading to prevent stuttering
- Resource reuse mechanism

### Extensibility
- Modular design for easy extension
- Interface-based component system
- Configuration-driven feature customization

### Development Support
- Editor extension tools
- Scene template support
- Input system configuration tools

## Project Structure
```
Assets/
├── Core/           # Core System Implementation
│   ├── Manager Base & Interfaces
│   ├── Framework Bootstrap
│   └── Update Dispatch System
│
├── Manager/        # Functional Module Implementation
│   ├── Audio/      # Audio Management
│   ├── Event/      # Event System
│   ├── GameFlow/   # Game Flow
│   ├── Input/      # Input Management
│   ├── ObjectPool/ # Object Pool
│   ├── Resource/   # Resource Management
│   ├── Scene/      # Scene Management
│   └── UI/         # UI System
│
├── Tools/          # Utility Classes
│   └── GameObject Tools
│
├── Editor/         # Editor Extensions
│   └── Asset Bundle Tool
│
├── Resources/      # Resource Configuration
│   ├── Audio Config
│   └── UI Resources
│
├── Settings/       # Project Settings
│   ├── Scene Templates
│   └── URP Configuration
│
├── Scenes/         # Scene Files
│   └── Sample Scene
│
└── Input/          # Input Configuration
    └── Input Actions
```
