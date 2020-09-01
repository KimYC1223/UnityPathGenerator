![Logo](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure1.gif?raw=true)



# Unity Curved Path Generator

**유니티 곡선 경로 생성기 V 1.0**

아래에 한국어 번역이 있습니다. (There is an Korean translation at the bottom.)

<br>

## 1. Introduction

Imagine that you make car running along curved road in Unity Scene.

You can make this car with Animator, but there are some problems.

<br>

#### 1-1 . It is difficult to handling object between keyframes.

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure2.png?raw=true)

Assum the object will move from red squre to blue squre.

There are so many ways to move object.

But If you implement this with Unity Animator, the Animator will choose the shortest path (orange line).

<br>

#### 1-2 . It is difficult to move at a constant speed

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure3.png?raw=true)

Suppose there is a path moving from a red square to a blue square through a green square as shown in the picture above.

When the distances of each point are not equal, if the animation keyframes are distributed as follows 1:1

![image](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure4.png?raw=true)

The speed at which the object moves in the two sections is different.

So, if you want to make constant speed animation  when s1 and s2 are not equal,

you should control keyframes to become S1 : S2 = ( t1 - t0 ) : ( t2 - t1 )

Of course, It's possible if you spend a lot of time.

But if the path is curved, It will be very hard to calculate the ratio.

<br>

#### 1-3 . Bézier Curve

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

## 2 . Environment

Unity Version : 2019.4.1f 

<br><br>

## 3 . How to use

There are 3 scripts I will introduce.

- **PathGenerator** : Script to make followable path the based on Bézier curve.
- **PathGeneratorGUI** : GUI Script to help generate path based on Bézier curve.
- **PathFollower** : Script to follow the path created by "Path Generator" class

So let's get started.

<br>

---

<br>

#### 3-1 . Import package

3-1-1 . Download [latest release unity package](https://github.com/KimYC1223/UnityPathGenerator/releases/tag/1.0) or clone this repo.

3-1-2. Import Unity package.

---

#### 3-2 . Generate Path

3-2-1 . In your scene, create empty gameobject. (and rename to "Path".)

3-2-2 . This object will be followable curved path.

3-2-3 . Add "Path Generate" script on this object.

![figure8](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure8.png?raw=true)

3-2-4 . Let's introduce parameters.

---

<br>

##### 3-2-4-1. **Flag**

​		Put in a game object (prefab) to represent the **"Flag".**

​		This object doesn't matter what it looks like, **but it shouldn't have a Collider.**

​		*(I recommend putting [Prefabs> Makers> Flag] to help you understand when you first start.)*

​		**The Flag is the point which is the objects must pass through.**

​		***(This is the same role as P0 and P2 in the picture above.)***

<br><br>

##### 3-2-4-2 . **Start Flag**

​		Put in a game object (prefab) to represent the **"Start Flag"**

​		This object doesn't matter what it looks like, **but it shouldn't have a Collider.**

​		*(I recommend putting [Prefabs> Makers> Start] to help you understand when you first start.)*

​		**Start Flag is the 0th Flag (FlagList[0]), which is the point where the object will start first.**

<br><br>

##### 3-2-4-3 . **Angle**

​		Put in a game object (prefab) to represent the **"Angle"**

​		This object doesn't matter what it looks like, **but it shouldn't have a Collider.**

​		*(I recommend putting [Prefabs> Makers> Angle] to help you understand when you first start.)*

​		**Angle is placed between two flags. As the angle changes, the shape of the curve changes. **

​		This determines the path of movement.

​		***(It is the same role as P1 in the picture above.)***

<br><br>

##### 3-2-4-4 . **Guide**

​		Put in a game object (prefab) to represent the **"Guide (Path)"**

​		This object doesn't matter what it looks like, **but it shouldn't have a Collider.**

​		*(I recommend putting [Prefabs> Makers> way] to help you understand when you first start.)*

​		**Guide visually expresses the path determined through Flags and Angles.**

<br><br>

##### 3-2-4-5 . **Is Close**

​		choose path type (Closed path or open path).

​		If this is true, auto connection between head of Flag List and tail of Flag List.

<br><br>

##### 3-2-4-6 . **Is Debug Obejct**

​		Determine to show Debug Objects (Flag,StarFlag,Angle).

<br><br>

##### 3-2-4-7 . **Is Debug Line**

​		Determine to show Debug Lines (Guide).

<br><br>

##### 3-2-4-8 . **Path Density**

![figure11](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure11.gif?raw=true)

​		Decide how accurately you will draw the curve.

​		Higher numbers are closer to the ideal curve, **but too high can cause objects to behave erratically.**

​		**Path Density should always be at least 2.**

​		The recommended value is 30.

<br><br>

##### 3-2-4-9 . **FlagList**

​		List of Flag. FlagList's Length should always be at least 2.

<br><br>

##### 3-2-4-10 . **AngleList**

​		List of Angle. AngleList's Length should always be equal FlagList's Length.

<br>

---

3-2-5 . Fill in the blanks. In this tutorial, I'll use [Prefabs>Makers] elements.

3-2-6 . Choose the number of Flag List. If determine the number, the script automatically create objects. 

![figure10](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure10.gif?raw=true)

3-2-7 . You can edit path to contol makers.

![figure9](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure9.gif?raw=true)

3-2-8 . Make your own path!

![figure12](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure12.gif?raw=true)

---

#### 3-3 . Move Object

3-3-1 . Create object to move.

3-3-2 . Add "Path Follower" script.

3-3-3. This object should have a "Rigidbody" Component. (If it doesn't have it, automatically add.)

![image-20200901145640494](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure14.png?raw=true)

3-3-4 . Let's introduce parameters.

---

##### 3-3-4-1 . **EndEvent()**

​		Input the function to run when it arrived final destination.

​		**Remember!** If the path is closed loop, there is no final destination.

<br><br>

##### 3-3-4-2 . **Path**

​		Input the object with "Path Generator" component.

​		This object will move along this path.

<br><br>

##### 3-3-4-3 . **Speed**

​		Speed of movement. **too high value can cause objects to behave erratically.**

<br><br>

##### 3-3-4-4 . **Turning Speed**

​		Speed of  rotation.  **too high value can cause objects to behave erratically.** 

<br><br>

##### 3-3-4-5 . **Is Loop**

​		If the path is opened and this value is true,

​		the object teleport at Start Flag when it arrived final destination.

​		**Remember!** If the path is closed loop, there is no final destination.

<br><br>

##### 3-3-4-6 . **IsMove**

​		If this value is false, object doesn't move.

<br>

---

3-3-5 . Fill the blanks.

3-3-6 . Object will move along the path.

![figure13](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure13.gif?raw=true)

---



## 4 . Examples

#### 4-1 . Auto Driving

There may be many examples, but the best example is to create an object that runs along a given track.

![figure15](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure15.gif?raw=true)

I once made a car that runs on a circular track I got from the Asset Store.

![figure16](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure16.gif?raw=true)

With a little modification, you can implement a car that moves naturally even the wheels and steering wheel.

<br>

---

<br>

#### 4-2 . Planet Movement

![figure17](https://github.com/KimYC1223/UnityPathGenerator/blob/master/ReadmeImage/figure17.gif?raw=true)

You can express movements between planets like the solar system.

This script also allows you to create circular paths and elliptical orbits like Comet Halley.

<br>

---

<br>

## 5 . QnA

#### 5-1 . My Git Blog

Thanks for read! check out [my blog](https://kimyc1223.github.io/) too !

<br>

#### 5-2 . Contact

- Create issue in [this repo](https://github.com/KimYC1223/UnityPathGenerator/issues)
- kau_esc@naver.com
- kimyc1223@gmail.com
- kim.youngchan@yonsei.ac.kr

<br><br><br>


---



**한국어 번역은 추후 추가 예정**

*Korean translation will be added later*
