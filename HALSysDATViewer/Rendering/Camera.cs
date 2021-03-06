﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace HALSysDATViewer.Rendering
{
    public class Camera
    {
        // Values from camera controls or stprm.bin.
        public Vector3 position = new Vector3(0, 7, -70);
        public float rotX = 0;
        public float rotY = 0;
        public float fovRadians = 0.524f;
        public float renderDepth = 100000;

        public int renderWidth = 1;
        public int renderHeight = 1;

        public Vector3 scale = new Vector3(1);

        // Matrices for rendering.
        public Matrix4 modelViewMatrix = Matrix4.Identity;
        public Matrix4 mvpMatrix = Matrix4.Identity;
        public Matrix4 projectionMatrix = Matrix4.Identity;
        public Matrix4 billboardMatrix = Matrix4.Identity;
        public Matrix4 billboardYMatrix = Matrix4.Identity;
        public Matrix4 rotationMatrix = Matrix4.Identity;
        public Matrix4 translation = Matrix4.Identity;
        public Matrix4 perspFov = Matrix4.Identity;

        // Camera control settings. 
        public float zoomMultiplier = 1;
        public float zoomSpeed = 5;
        public float mouseTranslateSpeed = 0.5f;
        public float scrollWheelZoomSpeed = 1.75f;
        public float shiftZoomMultiplier = 2.5f;
        public float mouseSLast = 0;
        public float mouseYLast = 0;
        public float mouseXLast = 0;

        public Camera()
        {

        }

        public Camera(Vector3 position, float rotX, float rotY, int renderWidth = 1, int renderHeight = 1)
        {
            this.position = position;
            this.renderHeight = renderHeight;
            this.renderWidth = renderWidth;
            this.rotX = rotX;
            this.rotY = rotY;
        }

        public void Update()
        {
            try
            {
                OpenTK.Input.Mouse.GetState();

                // Left click drag to rotate. Right click drag to pan.
                if ((OpenTK.Input.Mouse.GetState().RightButton == OpenTK.Input.ButtonState.Pressed))
                {
                    // Find the change in normalized screen coordinates.
                    float deltaYNormalized = (OpenTK.Input.Mouse.GetState().Y - mouseYLast) / renderHeight;
                    float deltaXNormalized = (OpenTK.Input.Mouse.GetState().X - mouseXLast) / renderWidth;

                    // Translate the camera based on the distance from the origin and field of view.
                    // Objects will "follow" the mouse while panning.
                    position.Y += deltaYNormalized * ((float)Math.Sin(fovRadians) * position.Length);
                    position.X += deltaXNormalized * ((float)Math.Sin(fovRadians) * position.Length);
                }
                if ((OpenTK.Input.Mouse.GetState().LeftButton == OpenTK.Input.ButtonState.Pressed))
                {
                    rotY += 0.0125f * (OpenTK.Input.Mouse.GetState().X - mouseXLast);
                    rotX += 0.005f * (OpenTK.Input.Mouse.GetState().Y - mouseYLast);
                }

                // Increase zoom speed when zooming out. 
                float zoomDistanceScale = 0.01f;
                float zoomscale = zoomSpeed * Math.Abs(position.Z) * zoomDistanceScale;

                // Holding shift changes zoom speed.
                if (OpenTK.Input.Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.ShiftLeft) || OpenTK.Input.Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.ShiftRight))
                    zoomscale *= shiftZoomMultiplier;

                // Zooms in or out with arrow keys.
                if (OpenTK.Input.Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.Down))
                    position.Z -= 1 * zoomscale;
                if (OpenTK.Input.Keyboard.GetState().IsKeyDown(OpenTK.Input.Key.Up))
                    position.Z += 1 * zoomscale;

                // Scroll wheel zooms in or out.
                position.Z += (OpenTK.Input.Mouse.GetState().WheelPrecise - mouseSLast) * zoomscale * scrollWheelZoomSpeed;

                // Update the mouse values. 
                TrackMouse();
            }
            catch (Exception)
            {
                // RIP OpenTK...
            }

            UpdateMatrices();
        }

        private void UpdateMatrices()
        {
            translation = Matrix4.CreateTranslation(position.X, -position.Y, position.Z);
            rotationMatrix = Matrix4.CreateRotationY(rotY) * Matrix4.CreateRotationX(rotX);
            perspFov = Matrix4.CreatePerspectiveFieldOfView(fovRadians, renderWidth / (float)renderHeight, 1.0f, renderDepth);

            modelViewMatrix = rotationMatrix * translation;
            mvpMatrix = modelViewMatrix * perspFov * Matrix4.CreateScale(scale);
            billboardMatrix = translation * perspFov;
            billboardYMatrix = Matrix4.CreateRotationX(rotX) * translation * perspFov;
        }

        public void TrackMouse()
        {
            mouseXLast = OpenTK.Input.Mouse.GetState().X;
            mouseYLast = OpenTK.Input.Mouse.GetState().Y;
            mouseSLast = OpenTK.Input.Mouse.GetState().WheelPrecise;
        }

        public void ResetPositionRotation()
        {
            position = new Vector3(0, 10, -80);
            rotX = 0;
            rotY = 0;
        }

        public void FrameSelection(Vector3 center, float radius)
        {
            // Calculate a right triangle using the bounding box radius as the height and the fov as the angle.
            // The distance is the base of the triangle. 
            float distance = radius / (float)Math.Tan(fovRadians / 2.0f);

            float offset = 10 / fovRadians;
            rotX = 0;
            rotY = 0;
            position.X = -center.X;
            position.Y = center.Y;
            position.Z = -1 * (distance + offset);
        }
    }
}
