![Logo](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/Title_Main.gif?raw=true)

---

<br>

# 📌 Patch Note ( Update V 2.0.2 / Nov 08 2023 )

- Change namespace (`PathGenerator` > `CurvedPathGenerator`)
- Edit asset hierarchy
- Minor bug fixes

<br>

---

# 🗺 Unity Curved Path Generator V 2.0

**🗺 유니티 곡선 경로 생성기 V 2.0**

아래에 [한국어 번역](https://github.com/KimYC1223/UnityPathGenerator#-%ED%8C%A8%EC%B9%98%EB%85%B8%ED%8A%B8--%EC%97%85%EB%8D%B0%EC%9D%B4%ED%8A%B8-v-200-)이 있습니다. (There is an [Korean translation](https://github.com/KimYC1223/UnityPathGenerator#-%ED%8C%A8%EC%B9%98%EB%85%B8%ED%8A%B8--%EC%97%85%EB%8D%B0%EC%9D%B4%ED%8A%B8-v-200-) at the bottom.)

<br><br>

---

<br><br>

# 💠 1. Introduction

Imagine that you make car running along curved road in Unity Scene.

You can make this car with Animator, but there are some problems.

<br>

## 🔶 1-1 . It is difficult to handling object between keyframes.

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure2.png?raw=true)

Assum the object will move from red squre to blue squre.

There are so many ways to move object.

But If you implement this with Unity Animator, the Animator will choose the shortest path (orange line).

---

## 🔶 1-2 . It is difficult to move at a constant speed

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure3.png?raw=true)

Suppose there is a path moving from a red square to a blue square through a green square as shown in the picture above.

When the distances of each point are not equal, if the animation keyframes are distributed as follows 1:1

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure4.png?raw=true)

The speed at which the object moves in the two sections is different.

So, if you want to make constant speed animation  when s1 and s2 are not equal,

you should control keyframes to become S1 : S2 = ( t1 - t0 ) : ( t2 - t1 )

Of course, It's possible if you spend a lot of time.

But if the path is curved, It will be very hard to calculate the ratio.

---

## 🔶 1-3 . Bézier Curve

The common issue of problems 1-1 and 1-2 is'curve'.

I was looking for how to express curves in Unity, and I found something called Bezier curves.

![bezier-curve 01](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure5.gif?raw=true)

First, think of a point that moves a straight line.

There is a straight line and the point M is moving at a constant speed above it.

The trajectory of this point M is  drawn as a simple straight line.

At this time, t is a number indicating how far the line segment has been proportionally advanced.

Add another line here and place a point on it that moves like an M.

Then, the original point M is referred to as M0, and the new point is referred to as M1.

The rules for moving M0 and M1 are the same as before.

![bezier-curve 02](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure6.gif?raw=true)

Here you can draw one more line connecting M0 and M1.

The line naturally moves together when M0 and M1 move.

You can put the point on that line, and let that point be B.

And if you look at the trajectory drawn by point B, you can see that it becomes a curve drawn at a constant speed.

The trajectory drawn by point B is called a quadratic Bezier curve.

Below is the Bezier curve equation for time t.

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure7.png?raw=true)

As you increase the control point, you can create a 3rd, 4th, 5th order .. Bezier curve.

As the control point increases, you can create sophisticated or complex curves,

For this project, I thought that the quadratic Bezier curve was sufficient.

<br><br>

---

<br><br>

# 💠 2 . Environment

Unity Version : 2019.4.1f ↑

<br><br>

---

<br><br>

# 💠 3 . How to use

There are 2 scripts I will introduce.

- **PathGenerator** : Script to make followable path the based on Bézier curve.
- **PathFollower** : Script to follow the path created by "Path Generator" class.

So let's get started.

<br><br>

---

<br><br>

## 🔶 3-1 . Package import

3-1-1 . You can download the [latest released unity package](https://github.com/KimYC1223/UnityPathGenerator/releases/tag/1.0) or download [this repo](https://github.com/KimYC1223/UnityPathGenerator).

3-1-2. Import Unity package.

However, the following must be observed.

![](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure18.PNG?raw=true)

- The files ```PathFollowerGUI.cs```, ```PathGeneratorGUI.cs```, and ```PathGeneratorGUILanguage.cs``` must be imported into the project, and they should be in ```Assets/Editor/CurvedPathGenerator```.

![](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure19.PNG?raw=true)

- The files ```PathFollower icon.PNG```, ```PathFollowerGUI icon.PNG```, ```PathGenerator icon.PNG```, ```PathGeneratorGUI icon.PNG```, ```PathGeneratorGUILanguage icon. PNG```, ```PG_Anchor.PNG```, ```PG_End.PNG```, ```PG_Handler.PNG```, ```PG_Node.PNG```, and ```PG_Start.PNG``` must be imported into the project, and they should be in ```Assets/Gizmos/CurvedPathGenerator```.

![](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure20.PNG?raw=true)

- The files ```PathFollowerScriptImg.PNG``` and ```PathGeneratorScriptImg.PNG``` must be imported into the project, and they should be in ```Assets/CurvedPathGenerator/Resources/Logo```

<br><br>

---

<br><br>


## 🔶 3-2 . Generate Path

### 🔹 3-2-1 . Object creation

Create an empty game object in your scene. (And rename it to "Path".)

This object becomes a curved path that can be followed.

---

### 🔹 3-2-2 . 컴포넌트 추가

Add ```Path Generator``` component to this object.

![figure8](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure21.PNG?raw=true)

```Path Generator``` is largely divided into 6 parts.

| Category             | Description                                                                 |
:---------------------:|:----------------------------------------------------------------------------|
| Header part          | Determine the nature of the path.                                           |
| Node part            | Shows a list of nodes that determine the origin, waypoint, and destination. |
| Angle part           | Shows a list of angles that determine the shape of the curve.               |
| Full Control Part    | All nodes and angles can be collectively controlled.                        |
| Rendering part       | Created curves can be visualized.                                           |
| Editor-related parts | Editor settings for easy curve control                                      |

<br><br>

---

<br><br>

#### 🔘 3-2-2-1. **Header part**

The part that determines the nature of the Path.

![figure22](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure22.PNG?raw=true)

##### 3-2-2-1-1. ```Path Density```

``Path density```` determines how accurately the curve is drawn.
 
 ![figure11](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure11.gif?raw=true)

The higher the number, the closer the curve is to the ideal, **but too high and the object may behave erratically.**

**Important point, ```Path density``` must always be greater than or equal to 2!**

Recommended values are 30-50.

##### 3-2-2-1-2. ```Update path in runtime```

If the ```Update path in runtime``` item is ```True```, the path is updated every frame.

Through this, even if the path changes in runtime, it is applied immediately.

However, the amount of computation may increase.

##### 3-2-2-1-3. ```Change to closed/opened path```

Determines whether to connect the last node and the last node.

---

#### 🔘 3-2-2-2. **Node part**

Shows a list of nodes that determine the origin, waypoint, and destination

![figure23](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure23.PNG?raw=true)

##### 3-2-2-2-1. ```Create node```

Adds a node to the end of the list.
 
##### 3-2-2-2-2. ```Delete node``` : [-] Button

Remove the selected node.

##### 3-2-2-2-3. ```Edit node``` : Edit Button

Edit the selected node.

---

#### 🔘 3-2-2-3. **Angle part**

Shows a list of angles that determine the shape of the curve.

![figure24](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure24.PNG?raw=true)

##### 3-2-2-3-1. ```Edit Angle``` : Edit Button

Edit the selected angle.

---

##### 🔘 3-2-2-4. **Full Control Part**

All nodes and angles can be collectively controlled.

![figure33](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure33.PNG?raw=true)

##### 3-2-2-4-1. ``` X / Y / Z to 0```

Set the ``` X / Y / Z ``` values of all angles and nodes in this path to 0.

##### 3-2-2-4-2. ``` X / Y / Z equalization```

Average the ``` X / Y / Z ``` values of all angles and nodes in this path.

##### 3-2-2-4-3. ``` X / Y / Z to specific value```

Make the ``` X / Y / Z ``` values of all angles and nodes in this path a specific value.

---

##### 🔘 3-2-2-5. **Rendering part**

You can visualize the created curve as shown in the figure below.

![figure30](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure30.gif?raw=true)

> There is a bug where the rendering is not displayed normally when the curved path is sharply bent.

![figure25](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure25.PNG?raw=true)

##### 3-2-2-5-1. ```Generate path mesh in runtime```

If ```Generate path mesh in runtime``` is set to ```True```, create a mesh that can visualize the created curve.

##### 3-2-2-5-2. ```Texture of line mesh```

Texture of the mesh of the lines to be displayed.

If the texture has a pattern like the picture below, it is good to express the flow.

<img src="https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure27.png?raw=true" width="256px">

> Demo textures located at ```/Assets/CurvedPathGenerator/DemoScene/Textures/```

If you want the texture to repeat, you must set the ```Wrap Mode``` to ```Repeat```.

![figure26](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure26.png?raw=true)

Also, if you want to see the Material repeated in the Scene,

![figure28](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure28.png?raw=true)

You need to turn on ```Animated Materials```.

##### 3-2-2-5-3. ```Width of line mesh```

The width of the line mesh to be displayed

##### 3-2-2-5-4. ```Scroll speed```

Scroll speed of the line texture to render. Can be set from -100 to 100.

##### 3-2-2-5-5. ```Opacity```

Transparency of the line texture to be displayed.

##### 3-2-2-5-6. ```Filling```

Decide how far to draw the line mesh to be expressed. Can be set from 0 to 1

##### 3-2-2-5-7. ```Render queue```

Specifying the render queue order of materials

---

#### 🔘 3-2-2-6. **Editor related parts**

Editor settings for easy curve control

![figure29](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure29.PNG?raw=true)

##### 3-2-2-6-1. ```Normal mode```

This mode changes the Transform information (Position, Rotation, Scale) of the current object.

This mode appears when an object is selected in Unity.

##### 3-2-2-6-2. ```Individual```

This mode allows you to edit the position of nodes and angles, not the current object.

Each node and angle can be edited one by one.

##### 3-2-2-6-3. ```Total```

This mode allows you to edit the positions of nodes and angles as a whole, not the current object.

Selecting this mode allows you to control all nodes and angles at once.

> This can be useful when resetting the pivot of the path.

##### 3-2-2-6-4. ```Show labels```

If this option is ```True```, the label is visible in the scene.

##### 3-2-2-6-5. ```Show icons```

If this option is ```True```, the icon is shown in the scene.

##### 3-2-2-6-6. ```Change to top view mode```

You can switch to Top view mode looking down the scene from above.

##### 3-2-2-6-7. ```Guideline colors```

You can specify the color of the guidelines.

<br>

---

### 🔹 3-2-3 . Create a path

Use the above functions appropriately to create the path you want.

![figure31](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure31.gif?raw=true)

<br><br>

---

<br><br>

## 🔶 3-3 . Move Object

### 🔹 3-3-1 . Create object

Add an empty object to move.

This object becomes the object following the path created in 3-2.

### 🔹 3-3-2. Add component

Add a "Path Follower" component to that object.

![figure32](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure32.PNG?raw=true)

```Path Generator``` is largely divided into two parts.

| Category                  | Description                                            |
:--------------------------:|:-------------------------------------------------------|
| movement information part | determine the nature of the movement.                  |
| event part                | Defines an event that occurs when a path is completed. |

<br><br>

---

<br><br>

#### 🔘 3-3-2-1. movement information part

Define the characteristics of movement.

##### 3-3-2-1-1. Path

Specifies the path to move. Just select ```PathGenerator``` in Scene.

If ```Path``` is empty, ```Path Follower``` cannot be moved.

##### 3-3-2-1-2. Speed

Specifies the movement speed.

The object moves along the given path at this speed.

If you enter a value that is too fast, it may malfunction.

##### 3-3-2-1-3. Distance threshold

When the distance between the moving object and the next node becomes less than this value,

It determines that the object has arrived at its destination and moves to the next node.

If this value is too small or too large, it may malfunction.

##### 3-3-2-1-4. Turning Speed

is the rotational speed of the object.

##### 3-3-2-1-5. Is Move

If this value is ```false```, the object does not move.

##### 3-3-2-1-6. Is Loop

If this value is ``` true```, the object will repeat its path infinitely.

It doesn't matter if the path is closed or open.

---

#### 🔘 3-3-2-2. **Event part**

Defines a method to be executed when the route is completed.

##### 3-3-2-2-1. Execute a method

If this value is ```true```, the method is executed every time the route is completed.

At this time, if object's ```Is Loop``` is ```true```, the method is not executed forever.

(Because I don't think I've completed the route.)

<br>
<br>

---

<br><br>

# 💠 4 . Examples

## 🔶 4-1 . Auto Driving

There may be many examples, but the best example is to create an object that runs along a given track.

![figure15](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure15.gif?raw=true)

I once made a car that runs on a circular track I got from the Asset Store.

![figure16](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure16.gif?raw=true)

With a little modification, you can implement a car that moves naturally even the wheels and steering wheel.

<br><br>

---

<br><br>


## 🔶 4-2 . Planet Movement

![figure17](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure17.gif?raw=true)

You can express movements between planets like the solar system.

This script also allows you to create circular paths and elliptical orbits like Comet Halley.

<br><br>

---

<br><br>

# 💠 5 . QnA

## 🔶 5-1 . My Git Blog

Thanks for read! check out [my blog](https://kimyc1223.github.io/) too !

## 🔶 5-2 . Contact

- Create issue in [this repo](https://github.com/KimYC1223/UnityPathGenerator/issues)
- kau_esc@naver.com
- kimyc1223@gmail.com
- kim.youngchan@yonsei.ac.kr

<br><br><br>

---

<br><br><br>

![Logo](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/Title_Main.gif?raw=true)

---

<br>

# 📌 패치노트 ( 업데이트 V 2.0.2 / 2023 11 08 )

- 네임스페이스 변경 ( `PathGenerator` > `CurvedPathGenerator` )
- 에셋 하이어라키 수정
- 자잘한 버그 수정

<br>

---

# 🗺 유니티 곡선 경로 생성기 V 2.0

**🗺 Unity Curved Path Generator V 2.0**

There is an [English translation](https://github.com/KimYC1223/UnityPathGenerator#-path-note--update-v-200-) at the top. (상단에 [영어 번역](https://github.com/KimYC1223/UnityPathGenerator#-path-note--update-v-200-)이 있습니다.)

<br><br>

---

<br><br>

# 💠 1. Introduction

유니티에서 오브젝트를 곡선 및 직선 경로를 따라 움직이게 만드는 방법은 무엇일까?

아마 가장 쉽고 직관적인 방법은 애니메이션을 사용하는 것일것이다.

하지만 애니메이션을 통한 구현 방법은 아래와 같은 문제점이 있다.

---

## 🔶 1-1 . 키프레임 사이를 원하는 방법대로 조절하기 어렵다.

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure2.png?raw=true)

빨간 네모에서 파란 네모로 움직이는 애니메이션,

즉 Position의 변화를 애니메이션으로 만든다고 하면

경로는 위 그림처럼 무수히 많이 존재 할 수 있는데,

애니메이션에서는 항상 가장 빨리 변할 수 있는 방향 (주황색 패스) 으로만 정해진다.

<br><br>

---

<br><br>


## 🔶 1-2 . 일정한 속도로 움직이기 어렵다

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure3.png?raw=true)

만약 위 그림처럼 빨간 네모에서 초록색 네모로, 그 후 파란색 네모로 움직이는 패쓰가 있다고 하자.

이 때 각 지점사이의 거리가 같지 않다고 할 때,

애니메이션 키프레임이 다음과 같이 분포한다면 (1:1 로)

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure4.png?raw=true)

두 부분에서 물체가 움직이는 속도는 다를것이다.

따라서 위 상황처럼 s1과 s2가 같지 않을 때 일정한 속도의 애니메이션을 만들고 싶다면,

S1 : S2 = ( t1 - t0 ) : ( t2 - t1 ) 이 되도록 키프레임을 제어해야 한다.

물론 시간을 많이 투자하면 가능하겠지만,

경로가 구부러지거나 그 수가 많아 비율을 계산하기 힘든 상황에선 불편 할 수 밖에 없다.

<br><br>

---

<br><br>


## 🔶 1-3 . Bézier 곡선

위에서 알 수 있는 문제점 중 공통된 사항은 ‘곡선’이다.

어떻게 하면 곡선을 나타 낼 수 있을까 찾아보다가, 베지어 곡선(Bézier curve)이라는 것을 찾아내었다.

![bezier-curve 01](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure5.gif?raw=true)

일단 직선을 움직이는 점을 생각해본다.

하나의 직선이 있고 그 위를 점 M이 일정 속도로 이동하고 있다.

이 점 M의 궤적은 당연하지만 단순한 직선으로 그려진다.

이때 t는 선분 위를 비율적으로 얼마나 나아갔는지를 나타내는 수치다.

여기에 선을 하나 더 추가하고 그 위에 M처럼 이동하는 점을 놓아본다.

그리고 원래의 점 M을 M0로, 새로운 점을 M1으로 한다.

M0와 M1이 움직이는 규칙은 이전과 같다.

M1이라는 점이 하나 더 늘었다 하더라도 특별히 복잡해질 것은 없다.

![bezier-curve 02](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure6.gif?raw=true)

여기에서 M0와 M1을 잇는 선을 하나 더 그을 수 있다.

그 선은 M0와 M1이 이동하면 자연스럽게 함께 움직이게 된다.

그 선 위에 M0나 M1처럼 일정 속도로 이동하는 점을 놓을 수 있다. 그 점을 B라고 하자.

그리고 점 B가 그리는 궤적을 살펴보면, 곡선이 되는 모습을 볼 수 있다.

점 B가 그리는 궤적을 2차 베지어 곡선(Quadratic Bezier Curve)이라고 한다.

아래는 시간 t에 대한 베지어 곡선 식이다.

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure7.png?raw=true)

조절점을 늘릴수록 3차, 4차, 5차 .. 베지어 곡선을 만들 수 있다.

차수가 늘어날 수록 정교하거나 복잡한 곡선을 만들 수 있지만,

이번 프로젝트에서는 2차 베지어 곡선만으로도 충분하다고 생각했다.

아래는 3차 베지어 곡선이다.

<br><br>

---

<br><br>

# 💠 2 . 환경

Unity 버전 : 2019.4.1f 이상

<br><br>

---

<br><br>

# 💠 3 . How to use

소개할 스크립트는 다음 2가지다.

- **PathGenerator** : 베지어 곡선을 기반으로 따라갈 수 있는 경로를 만드는 스크립트.
- **PathFollower** : "PathGenerator" 클래스에 의해 생성된 경로를 따르기 위한 스크립트

<br><br>

---

<br><br>

## 🔶 3-1 . 패키지 임포트

3-1-1 . [최신의 release된 unity package](https://github.com/KimYC1223/UnityPathGenerator/releases/tag/1.0)를 다운로드 하거나, [이 repo]((https://github.com/KimYC1223/UnityPathGenerator).)를 다운받으면 된다. 

3-1-2. 유니티 패키지를 임포트한다.

단,다음과 같은 사항을 반드시 지켜야한다.

![](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure18.PNG?raw=true)

- ```PathFollowerGUI.cs```, ```PathGeneratorGUI.cs```, ```PathGeneratorGUILanguage.cs```라는 파일은 필수적으로 프로젝트에 임포트 되어야 하며, ```Assets/Editor/CurvedPathGenerator``` 라는 폴더에 있어야 한다.

![](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure19.PNG?raw=true)

- ```PathFollower icon.PNG```, ```PathFollowerGUI icon.PNG```, ```PathGenerator icon.PNG```, ```PathGeneratorGUI icon.PNG```, ```PathGeneratorGUILanguage icon.PNG```, ```PG_Anchor.PNG```, ```PG_End.PNG```, ```PG_Handler.PNG```, ```PG_Node.PNG```, ```PG_Start.PNG```라는 파일은 필수적으로 프로젝트에 임포트 되어야 하며, ```Assets/Gizmos/CurvedPathGenerator``` 라는 폴더에 있어야 한다.

![](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure20.PNG?raw=true)

- ```PathFollowerScriptImg.PNG```, ```PathGeneratorScriptImg.PNG``` 라는 파일은 필수적으로 프로젝트에 임포트 되어야 하며, ```Assets/CurvedPathGenerator/Resources/Logo``` 라는 폴더에 있어야 한다.

<br><br>

---

<br><br>


## 🔶 3-2 . Generate Path

### 🔹 3-2-1 . 객체 생성

Scene에 빈 게임 개체를 만든다. (그리고 "Path"로 이름을 바꾼다.)

이 개체는 따라갈 수 있는 곡선 경로가 된다.

---

### 🔹 3-2-2 . 컴포넌트 추가

이 개체에 ```Path Generator``` 컴포넌트 추가한다.

![figure8](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure21.PNG?raw=true)

```Path Generator```는 크게 6가지 부분으로 나뉜다.

| 분류            | 설명 |
:---------------:|:-----|
| 헤더 파트       | Path의 성질을 결정한다. |
| 노드 파트       | 출발지, 경유지, 도착지를 결정하는 노드 리스트를 보여준다. |
| 앵글 파트       | 곡선의 모양을 결정하는 앵글 리스트를 보여준다. |
| 전체 제어 파트   | 모든 노드와 앵글을 일괄적으로 제어 할 수 있다. |
| 렌더링 파트     | 만든 곡선을 가시화 할 수 있다. |
| 에디터 관련 파트 | 곡선을 쉽게 제어 할 수 있는 에디터 설정 |

<br><br>

---

<br><br>

#### 🔘 3-2-2-1. **헤더 파트**

Path의​	성질을 결정하는 부분.

![figure22](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure22.PNG?raw=true)

##### 3-2-2-1-1. ```Path Density```

```Path density```는 곡선을 얼마나 정확하게 그릴지 결정한다.
 
![figure11](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure11.gif?raw=true)

숫자가 높을수록 이상적인 곡선에 가깝지만 **너무 높으면 개체가 비정상적으로 작동할 수 있다.**

**중요한 점은, ```Path density```는 항상 2 이상이어야 한다!**

권장 값은 30~50.

##### 3-2-2-1-2. ```Update path in runtime```

```Update path in runtime```항목이 ```True```이면, 경로가 매 frame마다 갱신된다.

이를 통해 런타임에서 경로가 바뀌어도, 즉시 반영된다.

하지만 연산량이 증가 할 수 있다.

##### 3-2-2-1-3. ```Change to closed/opened path```

가장 마지막 Node와 끝 Node를 연결할지 결정한다.

---

#### 🔘 3-2-2-2. **노드 파트**

출발지, 경유지, 도착지를 결정하는 노드 리스트를 보여준다

![figure23](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure23.PNG?raw=true)

##### 3-2-2-2-1. ```Create node```

리스트의 끝에 노드를 추가한다.
 
##### 3-2-2-2-2. ```Delete node``` : [-] 버튼

선택한 노드를 제거한다.

##### 3-2-2-2-3. ```Edit node``` : Edit 버튼

선택한 노드의 값을 변경한다.

---

#### 🔘 3-2-2-3. **앵글 파트**

곡선의 모양을 결정하는 앵글 리스트를 보여준다.

![figure24](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure24.PNG?raw=true)

##### 3-2-2-3-1. ```Edit Angle``` : Edit 버튼

선택한 앵글의 값을 변경한다.

---

##### 🔘 3-2-2-4. **전체 제어 파트**

모든 노드와 앵글을 일괄적으로 제어 할 수 있다.

![figure33](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure33.PNG?raw=true)

##### 3-2-2-4-1. ```X/Y/Z to 0```

이 경로의 모든 앵글과 노드의 ```X/Y/Z```값을 0으로 만든다.

##### 3-2-2-4-2. ```X/Y/Z equalization```

이 경로의 모든 앵글과 노드의 ```X/Y/Z```값을 평균값으로 만든다.

##### 3-2-2-4-3. ```X/Y/Z to specific value```

이 경로의 모든 앵글과 노드의 ```X/Y/Z```값을 특정값으로 만든다.

---

##### 🔘 3-2-2-5. **렌더링 파트**

아래 그림처럼 만든 곡선을 가시화 할 수 있다.

![figure30](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure30.gif?raw=true)

> 곡선 경로가 급격히 꺾일 경우, 렌더링이 정상적으로 표현되지 않는 버그가 있다.

![figure25](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure25.PNG?raw=true)

##### 3-2-2-5-1. ```Generate path mesh in runtime```

```Generate path mesh in runtime```이 ```True```일 경우, 만든 곡선을 가시화 할 수 있는 mesh를 만든다.

##### 3-2-2-5-2. ```Texture of line mesh```

표현할 라인의 mesh의 Texture.

Texture가 아래 그림처럼 패턴을 가지고 있을 경우, 흐름을 표현하기 좋다.

<img src="https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure27.png?raw=true" width="256px">

> ```/Assets/CurvedPathGenerator/DemoScene/Textures/```에 위치한 데모용 텍스처

텍스처가 반복되길 원한다면, 반드시 ```Wrap Mode```를 ```Repeat```로 설정해야한다.

![figure26](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure26.png?raw=true)

또한, Scene에서도 해당 Material이 반복되는것을 보고싶다면

![figure28](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure28.png?raw=true)

```Animated Materials```를 켜주어야 한다.

##### 3-2-2-5-3. ```Width of line mesh```

표현할 라인 mesh의 너비

##### 3-2-2-5-4. ```Scroll speed```

표현할 라인 texture의 스크롤 속도. -100 ~ 100까지 설정 가능.

##### 3-2-2-5-5. ```Opacity```

표현할 라인 texture의 투명도.

##### 3-2-2-5-6. ```Filling```

표현할 라인 mesh를 어디까지 그릴지 결정. 0 ~ 1까지 설정 가능

##### 3-2-2-5-7. ```Render queue```

Material의 render queue 순서 지정

---

#### 🔘 3-2-2-6. **에디터 관련 파트**

곡선을 쉽게 제어 할 수 있는 에디터 설정

![figure29](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure29.PNG?raw=true)

##### 3-2-2-6-1. ```Normal mode```

현재 오브젝트의 Transform 정보(Position, Rotation, Scale)를 변경 하는 모드이다.

기존 Unity에서 오브젝트를 선택했을 때 나타나는 모드이다.

##### 3-2-2-6-2. ```Individual```

현재 오브젝트가 아닌, 노드와 앵글들의 위치를 편집 할 수 있는 모드이다.

각 노드와 앵글을 하나씩 편집 할 수 있다.

##### 3-2-2-6-3. ```Total```

현재 오브젝트가 아닌, 노드와 앵글들의 위치를 전체적으로 편집 할 수 있는 모드이다.

이 모드를 선택하면 전체 노드와 앵글을 한 번에 제어 할 수 있다.

> path의 pivot을 재설정 할 때 유용하게 사용 할 수 있다.

##### 3-2-2-6-4. ```Show labels```

이 옵션이 ```True```이면, Scene에서 Label이 보여진다.

##### 3-2-2-6-5. ```Show icons```

이 옵션이 ```True```이면, Scene에서 icon이 보여진다.

##### 3-2-2-6-6. ```Change to top view mode```

Scene을 위에서 내려다보는 Top view 모드로 전환 할 수 있다.

##### 3-2-2-6-7. ```Guideline colors```

가이드라인의 색상을 지정 할 수 있다.

<br>

---

### 🔹 3-2-3 . 경로를 만들기

위 기능들을 적절히 사용하여 원하는 path를 만들면 된다.

![figure31](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure31.gif?raw=true)

<br><br>

---

<br><br>


## 🔶 3-3 . Move Object

### 🔹 3-3-1 . 객체 만들기

움직일 빈 오브젝트를 추가한다.

이 객체는 3-2에서 만든 경로를 따라가는 오브젝트가 된다.

### 🔹 3-3-2. 컴포넌트 추가

그 객체에 "Path Follower" 컴포넌트를 추가한다.

![figure32](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure32.PNG?raw=true)

```Path Generator```는 크게 2가지 부분으로 나뉜다.

| 분류             | 설명                    |
:----------------:|:------------------------|
| 움직임 정보 파트 | 움직임의 성질을 결정한다. |
| 이벤트 파트      | path를 완주했을 때 발생하는 이벤트를 정의한다. |

<br><br>

---

<br><br>

#### 🔘 3-3-2-1. 움직임 정보 파트

움직임의 특성을 정의한다.

##### 3-3-2-1-1. Path

움직일 path를 지정한다. Scene에 있는 ```PathGenerator```를 선택하면 된다.

```Path```가 비어있을 경우, ```Path Follower```는 움직일 수 없다.

##### 3-3-2-1-2. Speed

움직일 속도를 지정한다.

물체는 이 속도로 주어진 path를 따라 움직인다.

너무 빠른 값을 입력하면, 오작동할 수 있다.

##### 3-3-2-1-3. Distance threshold

움직이는 물체와 다음 노드 사이의 거리가 이 값 이하로 될 경우,

물체가 목적지에 도착했다고 판단하여 다음 노드로 움직인다.

이 값이 너무 작거나 클 경우, 오작동 할 수 있다.

##### 3-3-2-1-4. Turning Speed

물체의 회전 속도이다.

##### 3-3-2-1-5. Is Move

이 값이 ```false```이면, 물체가 움직이지 않는다.

##### 3-3-2-1-6. Is Loop

이 값이 ``` true```이면, 물체가 경로를 무한히 반복해서 움직입니다.

경로가 닫힌경로인지, 열린경로인지와는 상관없습니다.

---

#### 🔘 3-3-2-2. **이벤트 파트**

경로를 완주했을 때, 실행시킬 메소드를 정의한다.

##### 3-3-2-2-1. Execute a methods

이 값이 ```true```이면, 경로를 완주했을 때 마다 메소드를 실행시킨다.

이때, 물체의 ```Is Loop```가 ```true```일 경우, 해당 메소드가 영원히 실행되지 않는다.

(경로를 완주했다고 생각하지 않기 때문이다.)

<br>
<br>

---

<br><br>

# 💠 4 . 예제

## 🔶 4-1 . 자동차 주행

많은 예가 있을 수 있지만 가장 좋은 예는 주어진 트랙을 따라 달리는 객체를 만드는 것이다.

![figure15](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure15.gif?raw=true)

에셋스토어에서 받은 원형 트랙을 달리는 자동차를 만든 적이 있다.

![figure16](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure16.gif?raw=true)

본 스크립트를 약간 수정하고 응용하여 바퀴와 핸들까지 자연스럽게 움직이는 자동차를 구현할 수 있다.

<br><br>

---

<br><br>


### 🔶 4-2 . 행성 궤도

![figure17](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure17.gif?raw=true)

태양계와 같은 행성 간의 움직임을 표현할 수 있다.

이 스크립트를 사용하면 핼리 혜성과 같은 타원 궤도를 생성할 수 도 있다.

<br><br>

---

<br><br>

# 💠 5 . QnA

## 🔶 5-1 . 깃 블로그

읽어주셔서 감사합니다. 제 [블로그](https://kimyc1223.github.io/)도 확인해보세요!

## 🔶 5-2 . 컨택트

- [이 repo 이슈 페이지](https://github.com/KimYC1223/UnityPathGenerator/issues)에 이슈를 등록하시면 됩니다
- kau_esc@naver.com
- kimyc1223@gmail.com
- kim.youngchan@yonsei.ac.kr

<br><br><br>
