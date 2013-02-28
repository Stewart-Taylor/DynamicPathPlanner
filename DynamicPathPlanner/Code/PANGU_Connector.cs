/*      PANGU_Connector Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class connects to PANGU directly
 * It makes use of the Pangu.dll which is a compiled PANGU socket client
 *
 * Last Updated: 28/02/2013
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

        //Socket Connection Functions
        //------------------------------------------------------------------------------------------------------------

        // Customised function provides a socket that can be used in C#
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void* getSocket([MarshalAs(UnmanagedType.LPStr)]String hName, int port);

        // Starts a PANGU network protocol session
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_start(void* sock);

        // Ends a PANGU network protocol session
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_finish(void* sock);

        // Sets angle for Image Viewport
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern char* pan_protocol_get_viewpoint_by_angle(void* sock, float x, float y, float z, float yw, float pi, float rl, ulong* size);

        // Sets angle for Image Viewport
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern char* pan_protocol_set_viewpoint_by_angle(void* sock, float x, float y, float z, float yw, float pi, float rl);

        // Sets field of view for Image Viewport
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_set_field_of_view(void* sock, float f);

        // Sets aspect ration for Image Viewport
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_set_aspect_ratio(void* sock, float r);

        // Determines if boulders should appear in Image Viewport
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_set_boulder_view(void* sock, ulong t, int tex);

        // Gets Lidar Pulse result
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_get_lidar_pulse_result(void* sock, float x, float y, float z, float dx, float dy, float dz, float* r, float* a);
        
        // Quits the PANGU connection
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_quit(void* sock);

        // Gets the elevation for the array of points specified
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_get_surface_elevations(void* s, char boulders, ulong n, float* posv, float* resultv, char* errorv);

        // Gets the surface elevation for the array of points specified
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_get_elevations(void* s, ulong n, float* posv, float* resultv, char* errorv);

        // Gets the elevation for the point specified
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern float pan_protocol_get_elevation(void* s, char* perr);

        // Gets the Image from PANGU
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern char* pan_protocol_get_image(void* sock, ulong* s);

        // Gets an elevation patch
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void pan_protocol_get_surface_patch(void* sock, char boulders, float cx, float cy, ulong nx, ulong ny, float d, float theta, float* rv, char* ev);

        //Customised Elevation function, works faster
        [DllImport(@"PanguDLLs\DLLpangu.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public unsafe static extern void getElevationModel(void* s, char b, float d, int sizeX, int sizeY, float[] elevationGrid);
       
        //------------------------------------------------------------------------------------------------------------

        private bool connected = false;
        private unsafe void* sock;

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
                catch (Exception ex)
                {
                    return false;
                }
            }
   
        }

        //Uses getElevationModel function to get the DEM
        public double[,] getDEM(string fpath, float distance, int width, int height)
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
                }
                catch (Exception ex)
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
        public Bitmap getImage(float x, float y, float z, float yaw, float pitch, float roll)
        {
            unsafe
            {
                pan_protocol_set_aspect_ratio(sock, 1); //set aspect ratio
                pan_protocol_set_boulder_view(sock, 0, 0);//turn boulders off
                pan_protocol_set_field_of_view(sock, 30.0f);//set field of view

                ulong t = 1024;
                char* img;
                //  pan_protocol_set_viewpoint_by_angle(sock, 0, 0, 1866, -90, -90, 0); //set the image
                img = pan_protocol_get_viewpoint_by_angle(sock, x, y, z, yaw, pitch, roll, &t); //get the image
                UnmanagedMemoryStream readStream = new UnmanagedMemoryStream((byte*)img, (long)t);

          //      PPM ppm = new PPM(readStream);//convert the image
                readStream.Close(); //tidy up
                readStream.Dispose();//tidy up
           //     return ppm.getBitmap;
                return null;
            }
        }


        //returns connection status
        public bool getConnectionStatus
        {
            get
            {
                return connected;
            }
        }
    }
}