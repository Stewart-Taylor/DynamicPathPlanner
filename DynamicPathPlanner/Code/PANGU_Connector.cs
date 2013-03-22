/*      PANGU_Connector Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class connects to PANGU directly
 * It makes use of the Pangu.dll which is a compiled PANGU socket client written in C
 *
 * Last Updated: 16/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;

namespace DynamicPathPlanner.Code
{
    class PANGU_Connector
    {
        private bool connected = false;
        private unsafe void* sock;

        //Socket Connection Functions
        //------------------------------------------------------------------------------------------------------------

        // Customised function provides a socket that can be used in C#
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void* getSocket([MarshalAs(UnmanagedType.LPStr)]String hName, int port);

        // Starts a PANGU network protocol session
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_start(void* sock);

        // Ends a PANGU network protocol session
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_finish(void* sock);

        // Sets angle for Image Viewport
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern char* pan_protocol_get_viewpoint_by_angle(void* sock, float x, float y, float z, float yw, float pi, float rl, ulong* size);

        // Sets angle for Image Viewport
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern char* pan_protocol_set_viewpoint_by_angle(void* sock, float x, float y, float z, float yw, float pi, float rl);

        // Sets field of view for Image Viewport
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_set_field_of_view(void* sock, float f);

        // Sets aspect ration for Image Viewport
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_set_aspect_ratio(void* sock, float r);

        // Determines if boulders should appear in Image Viewport
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_set_boulder_view(void* sock, ulong t, int tex);

        // Gets Lidar Pulse result
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_get_lidar_pulse_result(void* sock, float x, float y, float z, float dx, float dy, float dz, float* r, float* a);
        
        // Quits the PANGU connection
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_quit(void* sock);

        // Gets the elevation for the array of points specified
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_get_surface_elevations(void* s, char boulders, ulong n, float* posv, float* resultv, char* errorv);

        // Gets the surface elevation for the array of points specified
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_get_elevations(void* s, ulong n, float* posv, float* resultv, char* errorv);

        // Gets the elevation for the point specified
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern float pan_protocol_get_elevation(void* s, char* perr);

        // Gets the Image from PANGU
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern char* pan_protocol_get_image(void* sock, ulong* s);

        // Gets an elevation patch
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_get_surface_patch(void* sock, char boulders, float cx, float cy, ulong nx, ulong ny, float d, float theta, float* rv, char* ev);

        //Customised Elevation function, works faster
        [DllImport(@"PanguDLLs\Pangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void getElevationModel(void* s, char b, float d, int sizeX, int sizeY, float[] elevationGrid);
       
        //------------------------------------------------------------------------------------------------------------

        public PANGU_Connector()
        {

        }

        public bool connect(string hostname, int port)
        {
            unsafe
            {
                byte[] host = new byte[hostname.Length];
                host = Encoding.ASCII.GetBytes(hostname + "\0"); // convert hostname
                
                try
                {
                    sock = getSocket(hostname, port); //Gets the socket

                    pan_protocol_start(sock); //Connects
                    connected = true;
                    return true;
                }
                catch 
                {
                    return false;
                }
            }
        }

        //Uses getElevationModel function to get the DEM
        public double[,] getDEM(float distance, int width, int height)
        {
            double[,] elevationModel = null;

            unsafe
            {
                //  float cx = 0.0f;
                //   float cy = 0.0f;
                int nx = width;
                int ny = height;
                // float theta = 0.0f;
                char b = '1';

                float[] elevationGrid = new float[(nx * ny)];
                elevationModel = new double[nx, ny];
                
                try
                {
                    getElevationModel(sock, b, distance, nx, ny, elevationGrid);

                    //Converts from 1D to 2D array
                    int temp = 0;
                    for (int x = 0; x < (int)nx; x++)
                    {
                        for (int y = 0; y < (int)ny; y++)
                        {

                            elevationModel[x, y] = elevationGrid[temp];
                            temp++;
                        }

                    }
                }
                catch 
                {
                    return null;
                }
            }

            return elevationModel;
        }


        public void disconnect()
        {
            unsafe
            {
                pan_protocol_finish(sock); // Disconnects
                connected = false;
                Console.WriteLine("Disconnected from PANGU server successfully");
            }
        }

        //returns bitmap image converted from the PANGU stream
        public Bitmap getImage(float x, float y, float z, float yaw, float pitch, float roll , float fov)
        {
            try
            {
                unsafe
                {
                    pan_protocol_set_aspect_ratio(sock, 1);     //sets aspect ratio
                    pan_protocol_set_boulder_view(sock, 1, 0);  //view boulders
                    pan_protocol_set_field_of_view(sock, fov);  //set field of view

                    ulong t = 1024;
                    char* img;
                    img = pan_protocol_get_viewpoint_by_angle(sock, x, y, z, yaw, pitch, roll, &t); //gets the image
                    UnmanagedMemoryStream readStream = new UnmanagedMemoryStream((byte*)img, (long)t);

                    Bitmap bitmap = fetchImage(readStream);
                    readStream.Close(); 
                    readStream.Dispose();
                    return bitmap;
                }
            }
            catch
            {
                //Error at PANGU end. 
                return null;
            }
        }

        private Bitmap fetchImage(UnmanagedMemoryStream memStream)
        {
            int buffer;
            string id ="";
            int width;
            int height;
            int max;
            byte[] rgbValues = null;
            Bitmap bitmap;

            do
            {
                buffer = memStream.ReadByte();
                id += (char)buffer;
            } while (buffer != '\n' && buffer != ' ');

            string dimension = "";
            do
            {
                buffer = memStream.ReadByte();
                dimension += (char)buffer;
            } while (buffer != '\n' && buffer != ' ');

            width = Convert.ToInt16(dimension);
            dimension = "";
            do
            {
                buffer = memStream.ReadByte();
                dimension += (char)buffer;
            } while (buffer != '\n' && buffer != ' ');

            height = Convert.ToInt16(dimension);
            string maxRGB = "";
            do
            {
                buffer = memStream.ReadByte();
                maxRGB += (char)buffer;
            } while (buffer != '\n' && buffer != ' ');

            max = Convert.ToInt16(maxRGB);
            rgbValues = new byte[height * width * 3];
            for (int i = 0; i < rgbValues.Length; i++)
            {
                rgbValues[i] = (byte)memStream.ReadByte();
            }

            memStream.Close();

            try
            {
                bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Rectangle rect = new Rectangle(0, 0, width, height);
                System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
                IntPtr ptr = bmpData.Scan0;

                byte[] bgrValues = new byte[rgbValues.Length];
                for (int i = 0; i < rgbValues.Length; i += 3)
                {
                    bgrValues[i] = rgbValues[i + 2];
                    bgrValues[i + 1] = rgbValues[i + 1];
                    bgrValues[i + 2] = rgbValues[i];
                }

                System.Runtime.InteropServices.Marshal.Copy(bgrValues, 0, ptr, bgrValues.Length);
                bitmap.UnlockBits(bmpData);
                return bitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating new bitmap: " + ex.ToString());
            }

            return null;
        }


        //returns connection status
        public bool getConnectionStatus()
        {
                return connected;
        }

    }
}