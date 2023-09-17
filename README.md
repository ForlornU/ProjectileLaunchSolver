# ProjectileLaunchSolver

![Screenshot](https://github.com/ForlornU/Images/blob/5733611600465fb97d9d2ad37690bb1352e48202/Thumbnail1.png)
![Screenshot](https://github.com/ForlornU/Images/blob/5733611600465fb97d9d2ad37690bb1352e48202/Thumbnail2.png)

This Unity project is kind of an archery/projectile simulation that showcases different targeting methods in the Unity3d engine. It offers three distinct targeting scripts

## Targeting Scripts
### 1. Simple Targeting

The `Simple Targeting` script represents the most basic targeting approach. It allows you to fire a projectile directly toward the target. This script works for scenarios where the target remains stationary and no prediction is required or you just want a very basic targeting approach.
- Very basic
- Direct aiming at the target.
- Suitable for stationary targets.

### 2. Tracking Targeting

The `Tracking Targeting` script predicts the target's position and fires an arrow to intercept it. This predictive approach is essential for hitting moving targets.
- Predictive targeting for moving targets.
- Anticipates the target's future position.
- Visualizes the estimated target trajectory.

### 3. Advanced Targeting

The `Advanced Targeting` script offers a sophisticated targeting systemwith physics-based calculations. It accounts for factors like gravity and allows you to control the randomness of the arrow's height while remaining highly accurate
- Physics-based targeting.
- Customizable arrow height randomness.
- Visualizes the arrow's trajectory with a LineRenderer.
- Displays targeting data on the user interface.

## Getting Started

To get started with this project, follow these steps:

1. Clone the repository to your local machine.
2. Open the project in Unity.
3. Explore the different targeting scripts in the `Assets/Scripts/Targeting` folder.
4. Attach the desired targeting script to your archer character or object.
5. Customize the script parameters to fit your game's requirements.
6. Play the scene

## Contributions

Contributions to this project are welcome. If you have ideas for improvements or new targeting methods, feel free to create a pull request.

## License
You are free to use, modify, and distribute the code as per the license terms.

Happy game development!

---

**Note:** This README provides a high-level overview of the project. For detailed code and usage instructions, refer to the individual script files and the Unity project itself.
